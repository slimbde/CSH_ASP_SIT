﻿using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public static class VotingEngine
	{
		private static int maxYear { get { using (var db = new ApplicationDbContext()) { return db.Vacations.Max(v => v.Year); } } }
		private static List<int> years = new List<int> { maxYear - 2, maxYear - 1, maxYear };
		private static Dictionary<int, Dictionary<int, double>> monthScore;
		private static SortedDictionary<double, ApplicationUser> userScore;


		private static Dictionary<int, Dictionary<int, double>> GetMonthScore()
		{
			var dbloc = new ApplicationDbContext();
			var result = new Dictionary<int, Dictionary<int, double>>();

			// считаем словарь год-месяц-очки
			foreach (var y in years)
			{
				result[y] = new Dictionary<int, double>();

				for (var m = 1; m < 13; ++m)
				{
					var exMonthScore = dbloc.MonthWeights.FirstOrDefault(M => M.Id == m).Score / dbloc.MonthWeights.FirstOrDefault(M => M.Id == m).MonthDays;
					var score = 0.0;

					if (y == maxYear - 2)
						score = exMonthScore * 0.5;
					else if (y == maxYear - 1)
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
			var dbloc = new ApplicationDbContext();

			var usrs = dbloc.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == dbloc.Units.FirstOrDefault(U => U.Id == unit).ChiefId)).ToList();
			var result = new SortedDictionary<double, ApplicationUser>();

			// обнуляем VacationRating и признак процесса голосования
			foreach (var usr in usrs)
			{
				dbloc.Votings.FirstOrDefault(v => v.UsrId == usr.Id).VacationRating = 0.0;
				dbloc.Votings.FirstOrDefault(v => v.UsrId == usr.Id).Voted = false;
			}
			dbloc.SaveChanges();


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
					var vacs = dbloc.Vacations.Where(v => v.Year == yr && v.UsrId == usr.Id).ToList();

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

						avgScore += userScore;
						++usrCount;
					}
				}

				// защита от деления на нуль, если в каком-то из расчетных годов
				// нет ни одной записи отпусков, т.е. счетчик usrCount==0
				avgScore = usrCount == 0 ? 0.0 : avgScore / usrCount;

				// присвоим новичкам средний балл
				rookies.ForEach(r =>
				{
					if (!annualScores.ContainsKey(r))
						annualScores.Add(r, avgScore);
					else
						annualScores[r] += avgScore;
				});
			}

			// перепишем в отсортированный список полученный annualScores
			var rnd = new Random();
			foreach (var item in annualScores)
			{
				if (!result.ContainsKey(item.Value))
					result.Add(item.Value, item.Key);
				else
					result.Add(item.Value + rnd.NextDouble() / 1000, item.Key);
			}

			return result;
		}
		private static void SetRating()
		{
			var dbloc = new ApplicationDbContext();

			var rating = 1;
			for (; rating < userScore.Count; ++rating)
			{
				var usrId = userScore.ElementAt(rating).Value.Id;
				dbloc.Votings.FirstOrDefault(v => v.UsrId == usrId).VacationRating = rating;
			}

			dbloc.SaveChanges();
		}



		public static void InitiateVoting(int unit)
		{
			// пересчитываем очки дней по годам
			monthScore = GetMonthScore();

			// очки каждого пользователя по трем годам упорядоченные по возрастанию
			userScore = GetUserScore(unit);

			// заполняем VacationRating по каждому пользователю
			SetRating();
		}
		public static void CancelVoting(int unit, int year)
		{
			var dbloc = new ApplicationDbContext();

			var unitSections = dbloc.Sections.Where(s => s.UnitId == unit).Select(sec => sec.Id);   // выбираем ид бюро
			var unitManagerId = dbloc.Units.FirstOrDefault(m => m.Id == unit).ChiefId;  // выбираем ид руководителя отдела
			var unitUsersId = dbloc.Users.Where(u => unitSections.Any(us => us == u.SectionId) || u.Id == unitManagerId).Select(uus => uus.Id); // фильтруем пользователей
			dbloc.Votings.Where(v => unitUsersId.Any(uid => uid == v.UsrId)).ToList().ForEach(V => { V.Voted = true; V.VacationRating = 0.0; }); // сносим им рейтинг и флаг голосования

			var vacsToCancel = dbloc.Vacations.Where(vac => vac.Year == year && unitUsersId.Any(ud => ud == vac.UsrId)); // фильтруем только нужных пользователей и год
			dbloc.Vacations.RemoveRange(vacsToCancel);

			dbloc.SaveChanges();
		}
		public static bool GetVotingStatus(int unit)
		{
			var dbloc = new ApplicationDbContext();

			var usrs = dbloc.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == dbloc.Units.FirstOrDefault(U => U.Id == unit).ChiefId));

			// ищем все заявки по данным пользователям
			var votes = dbloc.Votings.Where(v => usrs.Any(u => u.Id == v.UsrId));
			var vts = votes.ToList();
			return votes.Any(v => v.Voted == false);
		}
		public static string GetVotingUserName(int unit)
		{
			var dbloc = new ApplicationDbContext();
			var usrs = dbloc.Users.Where(u => (u.Section.UnitId == unit) || (u.Id == dbloc.Units.FirstOrDefault(U => U.Id == unit).ChiefId));
			var voterId = string.Empty;

			while (true)
			{
				// ищем все заявки среди не проголосовавших пользователей
				var votes = dbloc.Votings.Where(v => usrs.Any(u => u.Id == v.UsrId)).Where(v => v.Voted == false);

				if (votes.Count() > 0)
				{
					// находим пользователя с минимальным рейтингом из не проголосовавших
					voterId = votes.FirstOrDefault(v => v.VacationRating == votes.Min(V => V.VacationRating)).UsrId;

					var year = maxYear;
					var voterRating = votes.FirstOrDefault(vo => vo.UsrId == voterId).VacationRating;
					if (voterRating == 0)
						year = maxYear + 1;

					// проверяем сумму его отпусков
					var userVacs = dbloc.Vacations.Where(vac => vac.Year == year && vac.UsrId == voterId);
					if (userVacs != null && userVacs.Count() > 0)
					{
						int? sm = userVacs.Sum(us => us.Duration);
						// смотрим на результат суммы по отпускам этого юзера
						if (sm is null)
							break;

						// если сумма уже 28, выставляем что он уже проголосовал
						if (sm == 28)
						{
							votes.FirstOrDefault(vot => vot.UsrId == voterId).Voted = true;
							dbloc.SaveChanges();
						}
						else
							// и если нашли такого, у которого сумма меньше 28 - возвращаем его
							break;
					}
					else
						// если вакансий за макс год у пользователя еще нет - возвращаем его
						break;
				}
				else
					return "not started";
			}

			return usrs.FirstOrDefault(u => u.Id == voterId).FullName;
		}
		public static int GetUserUnit(IPrincipal User)
		{
			var dbloc = new ApplicationDbContext();
			var unit = 0;

			try
			{
				var usr = dbloc.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
				unit = (int)usr.Section.UnitId;
			}
			catch (Exception)
			{
				try
				{
					// если пользователь руководитель отдела и у него нет SectionId
					var chiefId = dbloc.Users.FirstOrDefault(us => us.UserName == User.Identity.Name).Id;
					unit = dbloc.Units.FirstOrDefault(u => u.ChiefId == chiefId).Id;
				}
				catch (Exception)
				{
					// если он и не руководитель (еще не зареган в бюро или админ)
					return -1;
				}
			}

			return unit;
		}
		public static int GetUserIdUnit(string userId)
		{
			var dbloc = new ApplicationDbContext();
			var unit = 0;

			try
			{
				var usr = dbloc.Users.FirstOrDefault(u => u.Id == userId);
				unit = (int)usr.Section.UnitId;
			}
			catch (Exception)
			{
				// если пользователь руководитель отдела и у него нет SectionId
				try
				{
					var chiefId = dbloc.Users.FirstOrDefault(us => us.Id == userId).Id;
					unit = dbloc.Units.FirstOrDefault(u => u.ChiefId == chiefId).Id;
				}
				catch (Exception)
				{
					// если он и не руководитель (еще не зареган в бюро или админ)
					return -1;
				}
			}

			return unit;
		}
		public static string GetUserFullName(IPrincipal User)
		{
			using (var db = new ApplicationDbContext())
				return db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName;
		}
	}
}