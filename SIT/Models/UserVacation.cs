using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public class UserVacation
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Days { get; set; }
		public double Score { get; set; }
	}
}