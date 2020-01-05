using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public class Section
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Название бюро")]
		public string Name { get; set; }


		[Display(Name = "Руководитель бюро")]
		public string ChiefId { get; set; }
		public virtual ApplicationUser Chief { get; set; }


		[Display(Name = "Отдел")]
		public int? UnitId { get; set; }
		public virtual Unit Unit { get; set; }


		[Display(Name = "Сотрудники")]
		public virtual ICollection<ApplicationUser> Workers { get; set; }


		public Section()
		{
			Workers = new List<ApplicationUser>();
		}
	}
}