using SIT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIT.Controllers
{
	public class HomeController : Controller
	{
		ApplicationDbContext db = new ApplicationDbContext();


		public ActionResult Index()
		{
			return View();
		}


		public ActionResult Help()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}