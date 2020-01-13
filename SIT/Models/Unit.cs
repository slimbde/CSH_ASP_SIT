using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public class Unit
	{
		ApplicationDbContext db = ApplicationDbContext.Create();


		public int Id { get; set; }

		[Required]
		[Display(Name = "Название отдела")]
		public string Name { get; set; }


		[Display(Name = "Руководитель отдела")]
		public string ChiefId { get; set; }
		public string GetUserFullName(string id) => db.Users.FirstOrDefault(u => u.Id == id).FullName;


		[Display(Name = "Бюро в отделе")]
		public virtual ICollection<Section> Sections { get; set; }
	}
}