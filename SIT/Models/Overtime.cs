using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public class Overtime
	{
		public int Id { get; set; }


		[Display(Name = "Сотрудник")]
		public string UsrId { get; set; }
		public virtual ApplicationUser Usr { get; set; }


		[Display(Name = "Дата")]
		public DateTime Date { get; set; }


		[Display(Name = "Переработка (мин.)")]
		[Range(1, 1200, ErrorMessage = "Допустимая продолжительность 1...1200 мин.")]
		public int Duration { get; set; }


		[Display(Name = "Использована")]
		public DateTime? Utilized { get; set; }

		public string UsrAddedId { get; set; }

		public string UsrAppliedId { get; set; }

		public static string GetUserId(IPrincipal user)
		{
			using (var db = ApplicationDbContext.Create())
				return db.Users.FirstOrDefault(u => u.UserName == user.Identity.Name).Id;
		}

		public static string GetUserFullName(string id)
		{
			if (id == null)
				return "";

			using (var db = ApplicationDbContext.Create())
				return db.Users.FirstOrDefault(u => u.Id == id).FullName;
		}
	}



	public class OvertimeListViewModel
	{
		public IEnumerable<Overtime> Overtimes { get; set; }
		public SelectList Units { get; set; }
		public SelectList Sections { get; set; }
		public SelectList Users { get; set; }
		public SelectList Years { get; set; }
		public SelectList Durations { get; set; }

		public OvertimeListViewModel(IPrincipal user, int? unit, int? section, string usrId, string year, int? duration, bool available)
		{
			User = user;
			Unit = unit;
			Section = section;
			UsrId = usrId;
			Year = year;
			Duration = duration;
			Available = available;

			db = ApplicationDbContext.Create();
			overtimes = db.Overtimes;

			if (User.IsInRole("chief"))
				handleChief();
			else if (User.IsInRole("manager"))
				handleManager();
			else if (User.IsInRole("admin"))
				handleAdmin();
			else
				handleSlave();

			handleTimePoint();

			assembleModel();
		}


		private void handleChief()
		{
			if (Unit == null || Unit == 0)
				Unit = VotingEngine.GetUserUnit(User); // -1 когда пользователь не прописан в бюро

			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.Add(db.Units.FirstOrDefault(unt => unt.Id == Unit));
			overtimes = overtimes.Where(o => o.Usr.Section.UnitId == Unit); // фильтр по отделу

			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			var chiefId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
			Section = db.Sections.FirstOrDefault(s => s.ChiefId == chiefId).Id;
			sections.Add(db.Sections.FirstOrDefault(s => s.Id == Section));

			if (Section != null && Section != 0)
				overtimes = overtimes.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.AddRange(db.Users.Where(u => u.SectionId == Section).OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				overtimes = overtimes.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleManager()
		{
			if (Unit == null || Unit == 0)
				Unit = VotingEngine.GetUserUnit(User);

			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.Add(db.Units.FirstOrDefault(unt => unt.Id == Unit));
			overtimes = overtimes.Where(v => v.Usr.Section.UnitId == Unit || v.UsrId == db.Units.FirstOrDefault(u => u.Id == Unit).ChiefId); // фильтр по отделу


			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			sections.AddRange(db.Sections.Where(s => s.UnitId == Unit).ToList());

			if (Section != null && Section != 0)
				overtimes = overtimes.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.AddRange(db.Users.Where(u => u.Section.UnitId == Unit).OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				overtimes = overtimes.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleAdmin()
		{
			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.AddRange(db.Units.ToList());

			if (Unit != null && Unit != 0)
				overtimes = overtimes.Where(v => v.Usr.Section.UnitId == Unit || v.UsrId == db.Units.FirstOrDefault(u => u.Id == Unit).ChiefId); // фильтр по отделу


			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			if (Unit != 0 && Unit != null)
				sections.AddRange(db.Sections.Where(s => s.UnitId == Unit).ToList());
			else
				sections.AddRange(db.Sections.ToList());

			if (Section != null && Section != 0)
				overtimes = overtimes.Where(v => v.Usr.SectionId == Section); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			if (Unit != 0 && Unit != null)
				users.AddRange(db.Users.Where(u => u.Section.UnitId == Unit).OrderBy(u => u.Surname).ToList());
			else
				users.AddRange(db.Users.OrderBy(u => u.Surname).ToList());

			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
				overtimes = overtimes.Where(v => v.UsrId == UsrId); // фильтр по пользователю
		}
		private void handleSlave()
		{
			if (Unit == null || Unit == 0)
				Unit = VotingEngine.GetUserUnit(User);

			// отделы
			units = new List<Unit> { new Unit { Id = 0, Name = "-- все --" } };
			units.Add(db.Units.FirstOrDefault(unt => unt.Id == Unit));
			overtimes = overtimes.Where(o => o.Usr.Section.UnitId == Unit); // фильтр по отделу

			// бюро
			sections = new List<Section> { new Section { Id = 0, Name = "-- все --" } };
			var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
			sections.Add(db.Sections.FirstOrDefault(s => s.Id == user.SectionId));

			overtimes = overtimes.Where(v => v.Usr.SectionId == user.SectionId); // фильтр по бюро

			// пользователи
			users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.Add(user);

			overtimes = overtimes.Where(v => v.UsrId == user.Id); // фильтр по пользователю
		}
		private void handleTimePoint()
		{
			// года
			var dates = overtimes.Select(v => v.Date.Year).Distinct().ToList();
			strYears = new List<string> { "-- все --" };
			dates.ForEach(item => strYears.Add(item.ToString()));

			if (!string.IsNullOrEmpty(Year) && Year != "-- все --")
			{
				var yr = Convert.ToInt32(Year);
				overtimes = overtimes.Where(v => v.Date.Year == yr); // фильтр по году
			}

			// длительности
			strDurations = new List<string> { "-- все --" };
			var durations = overtimes.Select(o => o.Duration).ToList();
			durations.ForEach(e => strDurations.Add(e.ToString()));

			if (Duration != null && Duration != 0)
				overtimes = overtimes.Where(ov => ov.Duration == Duration); // фильтр по продолжительности

			// использованные
			if (Available)
				overtimes = overtimes.Where(oo => oo.Utilized != null);
			else
				overtimes = overtimes.Where(oo => oo.Utilized == null);
		}
		private void assembleModel()
		{
			Overtimes = overtimes.OrderBy(o => o.Usr.Surname).ThenBy(ov => ov.Date).ToList();
			Units = new SelectList(units, "Id", "Name");
			Sections = new SelectList(sections, "Id", "Name");
			Users = new SelectList(users, "Id", "FullName");
			Years = new SelectList(strYears, "2020");
			Durations = new SelectList(strDurations);
		}

		private ApplicationDbContext db;
		private IPrincipal User;
		private int? Unit;
		private int? Section;
		private string UsrId;
		private string Year;
		private int? Duration;
		public bool Available;

		private IQueryable<Overtime> overtimes;
		private List<Unit> units;
		private List<Section> sections;
		private List<ApplicationUser> users;
		private List<string> strYears;
		private List<string> strDurations;
	}
}