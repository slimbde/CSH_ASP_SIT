using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIT.Models
{
	public class Vacation
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Сотрудник")]
		public string UsrId { get; set; }
		public ApplicationUser Usr { get; set; }

		[Display(Name = "Год")]
		public int Year { get; set; }

		[Display(Name = "Месяц")]
		public int Month { get; set; }

		[Display(Name = "Продолжительность")]
		public int Duration { get; set; }


		public float? Score { get; set; }
	}


	public class VacationListViewModel
	{
		public IEnumerable<Vacation> Vacations { get; set; }
		public SelectList Units { get; set; }
		public SelectList Users { get; set; }
		public SelectList Years { get; set; }
		public SelectList Months { get; set; }
	}
}