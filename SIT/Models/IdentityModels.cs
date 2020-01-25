using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System;

namespace SIT.Models
{
	// В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
	public class ApplicationUser : IdentityUser
	{
		[NotMapped]
		private ApplicationDbContext db = ApplicationDbContext.Create();


		[Display(Name = "Имя")]
		public string Name { get; set; }

		[Display(Name = "Фамилия")]
		public string Surname { get; set; }

		[Display(Name = "Отчество")]
		public string Patronic { get; set; }

		[Display(Name = "Таб. №")]
		public string TabNo { get; set; }

		[Display(Name = "Ф.И.О.")]
		public string FullName { get { return $"{Surname} {Name} {Patronic}"; } }

		[Display(Name = "Бюро")]
		public int? SectionId { get; set; }
		public virtual Section Section { get; set; }

		[Display(Name = "Субботники")]
		public bool ParticipateInLabour { get; set; }

		[Display(Name = "Медосмотр")]
		public DateTime? MedExam { get; set; }

		[Display(Name = "Охрана труда")]
		public DateTime? LabourSecurityExam { get; set; }

		[Display(Name = "Пром. безопасность")]
		public DateTime? IndustrialSecurityExam { get; set; }

		[Display(Name = "Каска")]
		public DateTime? GotHelmet { get; set; }

		[Display(Name = "Костюм")]
		public DateTime? GotSuit { get; set; }

		[Display(Name = "Ботинки")]
		public DateTime? GotBoots { get; set; }

		[Display(Name = "Куртка утепл.")]
		public DateTime? GotCoat { get; set; }

		[Display(Name = "Субботники")]
		public virtual ICollection<Labour> Labours { get; set; }

		[Display(Name = "Переработки")]
		public virtual ICollection<Overtime> Overtimes { get; set; }


		public bool MedExamOkay { get { return MedExam == null ? false : DateTime.Now.Year - MedExam.Value.Year < 2; } }
		public bool LabourSecurityExamOkay { get { return LabourSecurityExam == null ? false : DateTime.Now.Year - LabourSecurityExam.Value.Year < 3; } }
		public bool IndustrialSecurityExamOkay { get { return IndustrialSecurityExam == null ? false : DateTime.Now.Year - IndustrialSecurityExam.Value.Year < 3; } }
		public bool GotHelmetOkay { get { return GotHelmet == null ? false : DateTime.Now.Year - GotHelmet.Value.Year < 3; } }
		public bool GotSuitOkay { get { return GotSuit == null ? false : DateTime.Now.Year - GotSuit.Value.Year < 1; } }
		public bool GotBootsOkay { get { return GotBoots == null ? false : DateTime.Now.Year - GotBoots.Value.Year < 1; } }
		public bool GotCoatOkay { get { return GotCoat == null ? false : DateTime.Now.Year - GotCoat.Value.Year < 3; } }

		public bool IsManager
		{
			get
			{
				var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
				return userManager.IsInRole(Id, "manager");
			}
		}
		public bool IsAdmin
		{
			get
			{
				var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
				return userManager.IsInRole(Id, "admin");
			}
		}
		public bool IsChief
		{
			get
			{
				var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
				return userManager.IsInRole(Id, "chief");
			}
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Здесь добавьте утверждения пользователя
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public virtual DbSet<Unit> Units { get; set; }
		public virtual DbSet<Section> Sections { get; set; }
		public virtual DbSet<MonthWeight> MonthWeights { get; set; }
		public virtual DbSet<Vacation> Vacations { get; set; }
		public virtual DbSet<Voting> Votings { get; set; }
		public virtual DbSet<Labour> Labours { get; set; }
		public virtual DbSet<Overtime> Overtimes { get; set; }

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{ }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}