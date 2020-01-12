using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public class Labour
	{
		ApplicationDbContext db = ApplicationDbContext.Create();

		public int Id { get; set; }



		[Display(Name = "Дата")]
		public DateTime Date { get; set; }



		[Display(Name = "Участники")]
		public virtual ICollection<ApplicationUser> Users { get; set; }
	}



	public class LabourViewModel
	{
		internal class DateComparer : IComparer<ApplicationUser>
		{
			public int Compare(ApplicationUser x, ApplicationUser y)
			{
				long xMax = 0;
				try
				{
					xMax = x.Labours.Max(l => l.Date).ToFileTimeUtc();
				}
				catch (Exception) { }

				long yMax = 0;
				try
				{
					yMax = y.Labours.Max(l => l.Date).ToFileTimeUtc();
				}
				catch (Exception) { }

				if (xMax > yMax)
					return 1;
				else if (xMax < yMax)
					return -1;

				return 0;
			}
		}


		ApplicationDbContext db = ApplicationDbContext.Create();

		public virtual ICollection<ApplicationUser> Users { get; set; }
		public Unit GetUserUnit(ApplicationUser user)
		{
			Unit unit;
			try
			{
				unit = user.Section.Unit;
			}
			catch (Exception)
			{
				// если он и не прописан в бюро
				return null;
			}

			return unit;
		}
		public DateTime? GetUserLabourDate(ApplicationUser user)
		{
			var userLabours = Users.FirstOrDefault(u => u.Id == user.Id).Labours;
			if (userLabours.Count > 0)
				return userLabours.Max(l => l.Date);

			return null;
		}

		public LabourViewModel()
		{
			var users = db.Users.Where(u => u.ParticipateInLabour == true).ToList();
			users.Sort(new DateComparer());
			Users = users;
		}
	}



	public class LabourDeleteModel
	{
		public Labour Labour { get; private set; }
		public ApplicationUser User { get; private set; }

		public LabourDeleteModel(Labour labour, ApplicationUser user)
		{
			Labour = labour;
			User = user;
		}
	}
}