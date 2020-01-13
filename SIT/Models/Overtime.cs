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
	public class Overtime
	{
		public int Id { get; set; }


		[Display(Name = "Сотрудник")]
		public string UsId { get; set; }
		public virtual ApplicationUser Us { get; set; }


		[Display(Name = "Дата")]
		public DateTime Date { get; set; }


		[Display(Name = "Продолжительность")]
		public TimeSpan Duration { get; set; }
	}
}