using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public static class VotingEngine
	{
		private static ApplicationDbContext db = ApplicationDbContext.Create();
		private static int maxYear = db.Vacations.Max(v => v.Year);
		private static List<int> years = new List<int> { maxYear - 2, maxYear - 1, maxYear };
		private static Dictionary<int, Dictionary<int, double>> monthScore;
		private static SortedDictionary<double, ApplicationUser> userScore;


		private static Dictionary<int, Dictionary<int, double>> GetMonthScore()
		{
			var result = new Dictionary<int, Dictionary<int, double>>();

			// считаем словарь год-месяц-очки
			foreach (var y in years)
			{
				result[y] = new Dictionary<int, double>();

				for (var m = 1; m < 13; ++m)
				{
					var exMonthScore = db.MonthWeights.FirstOrDefault(M => M.Id == m).Score / db.MonthWeights.FirstOrDefault(M => M.Id == m).MonthDays;
					var score = 0.0;

					if (y == maxYear - 3)
						score = exMonthScore * 0.5;
					else if (y == maxYear - 2)
						score = exMonthScore * 0.7;
					else
						score = exMonthScore;

					result[y].Add(m, score);
				}
			}

			return result;
		}
		private static SortedDictionary<double, ApplicationUser> GetUserScore(int unit)
		{
			var usrs = db.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == db.Units.FirstOrDefault(U => U.Id == unit).ChiefId)).ToList();
			var result = new SortedDictionary<double, ApplicationUser>();

			// обнуляем VacationRating и признак процесса голосования
			foreach (var usr in usrs)
			{
				db.Votings.FirstOrDefault(v => v.UsrId == usr.Id).VacationRating = 0.0;
				db.Votings.FirstOrDefault(v => v.UsrId == usr.Id).Voted = false;
			}
			db.SaveChanges();


			// считаем очки по каждому отпуску, общие очки и пишем в базу
			var annualScores = new Dictionary<ApplicationUser, double>();

			foreach (var yr in years)
			{
				// лист новичков, которым пишется средний балл
				var rookies = new List<ApplicationUser>();
				var avgScore = 0.0;
				var usrCount = 0;

				foreach (var usr in usrs)
				{
					var userScore = 0.0;
					var vacs = db.Vacations.Where(v => v.Year == yr && v.UsrId == usr.Id).ToList();

					foreach (var vac in vacs)
					{
						var rate = monthScore[yr][vac.Month];
						vac.Score = vac.Duration * rate;
						userScore += (double)vac.Score;
					}

					// если очки нулевые - сохраним его в листе новичков
					if (userScore == 0)
						rookies.Add(usr);
					else
					{
						if (!annualScores.ContainsKey(usr))
							annualScores.Add(usr, userScore);
						else
							annualScores[usr] += userScore;
					}

					avgScore += userScore;
					++usrCount;
				}

				// присвоим новичкам средний балл
				rookies.ForEach(r =>
				{
					if (!annualScores.ContainsKey(r))
						annualScores.Add(r, avgScore / usrCount);
					else
						annualScores[r] += avgScore / usrCount;
				});
			}

			// перепишем в отсортированный список полученный annualScores
			var rnd = new Random();
			foreach (var item in annualScores)
			{
				if (!result.ContainsKey(item.Value))
					result.Add(item.Value, item.Key);
				else
					result.Add(item.Value + rnd.NextDouble() / 10000, item.Key);
			}

			return result;
		}
		private static void SetRating()
		{
			var rating = 1;
			for (; rating < userScore.Count; ++rating)
			{
				var usrId = userScore.ElementAt(rating).Value.Id;
				db.Votings.FirstOrDefault(v => v.UsrId == usrId).VacationRating = rating;
			}

			db.SaveChanges();
		}



		public static void InitiateVoting(int unit)
		{
			//// защита от повторного запуска
			//if (GetVotingStatus(unit))
			//	return;

			// пересчитываем очки дней по годам
			monthScore = GetMonthScore();

			// очки каждого пользователя по трем годам упорядоченные по возрастанию
			userScore = GetUserScore(unit);

			// заполняем VacationRating по каждому пользователю
			SetRating();
		}
		public static bool GetVotingStatus(int unit)
		{
			var usrs = db.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == db.Units.FirstOrDefault(U => U.Id == unit).ChiefId));

			// ищем все заявки по данным пользователям
			var votes = db.Votings.Where(v => usrs.Any(u => u.Id == v.UsrId));
			var vts = votes.ToList();
			return votes.Any(v => v.Voted == false);
		}
		public static string GetVotingUserName(int unit)
		{
			var usrs = db.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == db.Units.FirstOrDefault(U => U.Id == unit).ChiefId));

			// ищем все заявки среди не проголосовавших пользователей
			var votes = db.Votings.Where(v => usrs.Any(u => u.Id == v.UsrId)).Where(v => v.Voted == false);
			var votesCount = votes.Count();

			if (votesCount > 0)
			{
				var voterId = votes.FirstOrDefault(v => v.VacationRating == votes.Min(V => V.VacationRating)).UsrId;
				return usrs.FirstOrDefault(u => u.Id == voterId).FullName;
			}

			return "not started";
		}
	}
}