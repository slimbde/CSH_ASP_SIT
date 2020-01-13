using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public class Section
	{
		[NotMapped]
		ApplicationDbContext db = ApplicationDbContext.Create();


		public int Id { get; set; }

		[Required]
		[Display(Name = "Название бюро")]
		public string Name { get; set; }


		[Display(Name = "Руководитель бюро")]
		public string ChiefId { get; set; }
		//[ForeignKey("ChiefId")]
		//public virtual ApplicationUser Chief { get; set; }

		public string GetUserFullName(string id)
		{
			return db.Users.FirstOrDefault(u => u.Id == id).FullName;
		}


		[Display(Name = "Отдел")]
		public int? UnitId { get; set; }
		public virtual Unit Unit { get; set; }


		[Display(Name = "Сотрудники")]
		public ICollection<ApplicationUser> Staff { get; set; }
	}
}