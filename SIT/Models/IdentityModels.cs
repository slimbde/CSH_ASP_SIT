using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SIT.Models
{
	// В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Patronic { get; set; }


		[Display(Name = "Таб. №")]
		public string TabNo { get; set; }


		[Display(Name = "Ф.И.О.")]
		public string FullName { get { return $"{Surname} {Name} {Patronic}"; } }


		[Display(Name = "Рейтинг субботника")]
		public double? CleaningRating { get; set; }


		[Display(Name = "Бюро")]
		public int? SectionId { get; set; }
		public virtual Section Section { get; set; }


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


		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{ }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}