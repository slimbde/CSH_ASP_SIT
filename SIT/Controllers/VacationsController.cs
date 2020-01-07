using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIT.Models;

namespace SIT.Controllers
{
	public class VacationsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Vacations
		public ActionResult Index()
		{
			return View(db.Vacations.Where(v => v.Year == 2019).OrderBy(v => v.Usr.Surname).ThenBy(v => v.Month).Include(v => v.Usr).ToList());
		}

		[Authorize(Roles = "admin, manager, chief")]
		public ActionResult Review(int? unit, string UsrId, string Year, string Month)
		{
			var vacations = db.Vacations.Include(u => u.Usr);
			var units = new List<Unit> { new Unit { Name = "-- все --" } };
			units.AddRange(db.Vacations.Select(v => v.Usr.Section.Unit).Distinct().ToList());
			units[1] = new Unit { Name = "руководители", Id = 3 };
			switch (unit)
			{
				case 3: // managers
					vacations = vacations.Where(v => v.Usr.SectionId == null);
					break;
				case 0: // all of them
				case null:
					break;
				default:
					vacations = vacations.Where(v => v.Usr.Section.UnitId == unit);
					break;
			}

			var users = new List<ApplicationUser> { new ApplicationUser { Id = "-- все --", Surname = "-- все --" } };
			users.AddRange(db.Users.OrderBy(u => u.Surname).ToList());
			if (!string.IsNullOrEmpty(UsrId) && UsrId != "-- все --")
			{
				vacations = vacations.Where(v => v.UsrId == UsrId);
			}


			var years = vacations.Select(v => v.Year).Distinct().ToList();
			var strYears = new List<string> { "-- все --" };
			foreach (var item in years)
			{
				strYears.Add(item.ToString());
			}
			if (!string.IsNullOrEmpty(Year) && Year != "-- все --")
			{
				int year = Convert.ToInt32(Year);
				vacations = vacations.Where(v => v.Year == year);
			}


			var months = vacations.Select(v => v.Month).Distinct().ToList();
			var strMonths = new List<string> { "-- все --" };
			foreach (var item in months)
			{
				strMonths.Add(item.ToString());
			}
			if (!string.IsNullOrEmpty(Month) && Month != "-- все --")
			{
				int month = Convert.ToInt32(Month);
				vacations = vacations.Where(v => v.Month == month);
			}


			var vlvm = new VacationListViewModel
			{
				Vacations = vacations.OrderBy(v => v.Usr.Surname).ToList(),
				Units = new SelectList(units, "Id", "Name"),
				Users = new SelectList(users, "Id", "FullName"),
				Years = new SelectList(strYears),
				Months = new SelectList(strMonths),
			};

			return View(vlvm);
		}


		// GET: Vacations/Create
		public ActionResult Create()
		{
			ViewBag.UserList = new SelectList(db.Users.OrderBy(u => u.Surname), "Id", "FullName");
			return View();
		}

		// POST: Vacations/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,UsrId,Duration")] Vacation vacation, DateTime date)
		{
			if (ModelState.IsValid)
			{
				vacation.Year = date.Year;
				vacation.Month = date.Month;

				db.Vacations.Add(vacation);
				await db.SaveChangesAsync();
				return RedirectToAction("Review");
			}

			ViewBag.UserList = new SelectList(db.Users.OrderBy(u => u.Surname), "Id", "FullName");
			return View(vacation);
		}

		// GET: Vacations/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vacation vacation = await db.Vacations.FindAsync(id);
			if (vacation == null)
			{
				return HttpNotFound();
			}
			return View(vacation);
		}

		// POST: Vacations/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,Year,Month,Duration,Score")] Vacation vacation)
		{
			if (ModelState.IsValid)
			{
				db.Entry(vacation).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(vacation);
		}

		// GET: Vacations/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vacation vacation = await db.Vacations.FindAsync(id);
			if (vacation == null)
			{
				return HttpNotFound();
			}
			vacation.Usr = db.Users.FirstOrDefault(u => u.Id == vacation.UsrId);
			return View(vacation);
		}

		// POST: Vacations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Vacation vacation = await db.Vacations.FindAsync(id);
			db.Vacations.Remove(vacation);
			await db.SaveChangesAsync();
			return RedirectToAction("Review");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
