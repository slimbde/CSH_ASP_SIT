using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public class Unit
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Название отдела")]
		public string Name { get; set; }


		[Display(Name = "Руководитель отдела")]
		public string ChiefId { get; set; }
		public virtual ApplicationUser Chief { get; set; }


		[Display(Name = "Бюро в отделе")]
		public virtual ICollection<Section> Sections { get; set; }
	}
}