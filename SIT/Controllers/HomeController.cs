using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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


		public async Task<ActionResult> Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				return View(await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name));
			}

			return View();
		}


		public ActionResult Help()
		{
			return View();
		}
	}
}