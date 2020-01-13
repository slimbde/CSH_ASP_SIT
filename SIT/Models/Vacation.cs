using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public class Vacation
	{
		public int Id { get; set; }


		[Display(Name = "Сотрудник")]
		public string UsrId { get; set; }
		public virtual ApplicationUser Usr { get; set; }


		[Display(Name = "Год")]
		public int Year { get; set; }

		[Display(Name = "Месяц")]
		public int Month { get; set; }

		[Display(Name = "Продолжительность")]
		[Range(1, 28, ErrorMessage = "продолжительность д.б. в пределах 1...28 дней")]
		public int Duration { get; set; }


		public double? Score { get; set; }
	}


	public class VacationListViewModel
	{
		public IEnumerable<Vacation> Vacations { get; set; }
		public SelectList Units { get; set; }
		public SelectList Sections { get; set; }
		public SelectList Users { get; set; }
		public SelectList Years { get; set; }
		public SelectList Months { get; set; }

		public VacationListViewModel(IPrincipal user, int? unit, int? section, string usrId, string year, string month)
		{
			User = user;
			Unit = unit;
			Section = section;
			UsrId = usrId;
			Year = year;
			Month = month;

			db = ApplicationDbContext.Create();
			vacations = db.Vacations;

			if (User.IsInRole("chief"))
				handleChief();
			else if (User.IsInRole("manager"))
				handleManager();
			else
				handleAdmin();

			handleTimePoint();

			assembleModel();
		}


		private void assembleModel()
		{
			Vacations = vacations.OrderBy(v => v.Usr.Surname).ToList();
			Units = new SelectList(units, "Id", "Name");
			Sections = new SelectList(sections, "Id", "Name");
			Users = new SelectList(users, "Id", "FullName");
			Years = new SelectList(strYears);
			Months = new SelectList(strMonths);
		}
		private void handleChief()
		{
			if (Unit == null || Unit == 0)
				Unit = VotingEngine.GetUserUnit(User);

			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.Add(db.Units.FirstOrDefault(unt => unt.Id == Unit));
			vacations = vacations.Where(v => v.Usr.Section.UnitId == Unit); // фильтр по отделу

			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			var chiefId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
			Section = db.Sections.FirstOrDefault(s => s.ChiefId == chiefId).Id;
			sections.Add(db.Sections.FirstOrDefault(s => s.Id == Section));

			if (Section != null && Section != 0)
				vacations = vacations.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.AddRange(db.Users.Where(u => u.SectionId == Section).OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				vacations = vacations.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleManager()
		{
			if (Unit == null || Unit == 0)
				Unit = VotingEngine.GetUserUnit(User);

			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.Add(db.Units.FirstOrDefault(unt => unt.Id == Unit));
			vacations = vacations.Where(v => v.Usr.Section.UnitId == Unit || v.UsrId == db.Units.FirstOrDefault(u => u.Id == Unit).ChiefId); // фильтр по отделу


			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			sections.AddRange(db.Sections.Where(s => s.UnitId == Unit).ToList());

			if (Section != null && Section != 0)
				vacations = vacations.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.AddRange(db.Users.Where(u => u.Section.UnitId == Unit).OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				vacations = vacations.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleAdmin()
		{
			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.AddRange(db.Units.ToList());

			if (Unit != null && Unit != 0)
				vacations = vacations.Where(v => v.Usr.Section.UnitId == Unit || v.UsrId == db.Units.FirstOrDefault(u => u.Id == Unit).ChiefId); // фильтр по отделу


			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			if (Unit != 0 && Unit != null)
				sections.AddRange(db.Sections.Where(s => s.UnitId == Unit).ToList());
			else
				sections.AddRange(db.Sections.ToList());

			if (Section != null && Section != 0)
				vacations = vacations.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			if (Unit != 0 && Unit != null)
				users.AddRange(db.Users.Where(u => u.Section.UnitId == Unit).OrderBy(u => u.Surname).ToList());
			else
				users.AddRange(db.Users.OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				vacations = vacations.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleTimePoint()
		{
			// года
			var years = vacations.Select(v => v.Year).Distinct().ToList();
			strYears = new List<string> { "-- все --" };
			foreach (var item in years)
			{
				strYears.Add(item.ToString());
			}
			if (!string.IsNullOrEmpty(Year) && Year != "-- все --")
			{
				int year = Convert.ToInt32(Year);
				vacations = vacations.Where(v => v.Year == year);
			}

			// месяцы
			var months = vacations.Select(v => v.Month).Distinct().ToList();
			strMonths = new List<string> { "-- все --" };
			foreach (var item in months)
			{
				strMonths.Add(item.ToString());
			}
			if (!string.IsNullOrEmpty(Month) && Month != "-- все --")
			{
				int month = Convert.ToInt32(Month);
				vacations = vacations.Where(v => v.Month == month);
			}
		}

		private ApplicationDbContext db;
		private IPrincipal User;
		private int? Unit;
		private int? Section;
		private string UsrId;
		private string Year;
		private string Month;

		private IQueryable<Vacation> vacations;
		private List<Unit> units;
		private List<Section> sections;
		private List<ApplicationUser> users;
		private List<string> strYears;
		private List<string> strMonths;
	}


	public class VacationVotingViewModel
	{
		// Словарь дней по конкретному пользователю
		public Dictionary<string, Dictionary<int, int>> UserMonthList { get; private set; }
		public Dictionary<string, double?> UserRating { get; private set; }
		public bool VotingStarted { get; private set; }
		public List<int> DaysOccupied { get; private set; }
		public int Year { get; private set; }
		public int UnitId { get; private set; }
		public int UnitUserCount { get => db.Users.Where(u => u.Section.UnitId == UnitId).Count() + 1; }
		public bool UsrIsVoter { get => VotingEngine.GetVotingUserName(UnitId) == db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName; }
		public string VoterFullName { get => VotingEngine.GetVotingUserName(UnitId); }
		public string UsrFullName { get => db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName; }
		public string GetFullName(string usrId) => db.Users.FirstOrDefault(u => u.Id == usrId).FullName;
		public List<Unit> GetUnitList() => db.Units.ToList();


		// CONSTRUCTOR
		public VacationVotingViewModel(int year, IPrincipal user, int unit, bool voting)
		{
			db = ApplicationDbContext.Create();
			Year = year;
			User = user;
			UnitId = unit;
			VotingStarted = voting;

			SetVacations();
			SetDaysOccupied();
			SetUserMonthList();
			SetUserRating();
		}


		private ApplicationDbContext db { get; set; }
		private List<Vacation> Vacations { get; set; }
		private IPrincipal User { get; set; }


		private void SetVacations()
		{
			IQueryable<Vacation> vacations = null;

			var unitChiefId = db.Units.FirstOrDefault(u => u.Id == UnitId).ChiefId;
			vacations = db.Vacations.Where(v => (v.Usr.Section.Unit.ChiefId == unitChiefId) || (v.Usr.Id == unitChiefId));

			Vacations = vacations.Where(v => v.Year == Year).OrderBy(v => v.Usr.Surname).ThenBy(v => v.Month).Include(v => v.Usr).ToList();
		}
		private void SetDaysOccupied()
		{
			DaysOccupied = new List<int>();
			for (int month = 1; month < 13; ++month)
				DaysOccupied.Add(Vacations.Where(m => m.Month == month).Sum(m => m.Duration));
		}
		private void SetUserMonthList()
		{
			UserMonthList = new Dictionary<string, Dictionary<int, int>>(Vacations.Count);

			// перебираем все выбранные отпуска и для каждого пользователя
			// консолидируем его дни по месяцам в этом листе
			for (var i = 0; i < Vacations.Count; ++i)
			{
				var usrId = Vacations[i].UsrId;

				if (!UserMonthList.ContainsKey(usrId))
					UserMonthList.Add(usrId, new Dictionary<int, int>(12));

				UserMonthList[usrId].Add(Vacations[i].Month, Vacations[i].Duration);
			}
		}
		private void SetUserRating()
		{
			UserRating = new Dictionary<string, double?>();

			for (var i = 0; i < UserMonthList.Count; ++i)
			{
				var usrId = UserMonthList.ElementAt(i).Key;
				UserRating.Add(usrId, db.Votings.FirstOrDefault(v => v.UsrId == usrId).VacationRating);
			}
		}
	}



	public class Voting
	{
		public int Id { get; set; }


		public string UsrId { get; set; }
		public virtual ApplicationUser Usr { get; set; }


		public double VacationRating { get; set; }


		public bool Voted { get; set; }
	}
}