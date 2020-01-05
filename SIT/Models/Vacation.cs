using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIT.Models
{
	public class Vacation
	{
		public int Id { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Duration { get; set; }
	}
}