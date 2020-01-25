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
	[Authorize]
	public class OvertimesController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Overtimes
		public ActionResult Index(int? unit, int? section, string usrId, int? duration, string available, string message = "", string year = "2020")
		{
			bool notUsed = available != null ? true : false;
			var model = new OvertimeListViewModel(User, unit, section, usrId, year, duration, notUsed);

			ViewBag.message = message;
			return View(model);
		}


		// GET: Overtimes/Create
		[Authorize(Roles = "admin, manager, chief")]
		public async Task<ActionResult> Create()
		{
			IQueryable<ApplicationUser> usrs = db.Users;
			var usrId = Overtime.GetUserId(User);
			var usrUnit = VotingEngine.GetUserUnit(User); // -1 если пользователь админ

			if (User.IsInRole("chief"))
				usrs = usrs.Where(us => us.Section.ChiefId == usrId);
			else if (User.IsInRole("manager"))
				usrs = usrs.Where(u => u.Section.UnitId == usrUnit || u.Id == usrId);
			else if (User.IsInRole("admin"))
				ViewBag.UsrList = new SelectList(await usrs.OrderBy(u => u.Surname).ToListAsync(), "Id", "FullName");

			return View();
		}

		// POST: Overtimes/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "UsrId,Date,Duration,Utilized,UsrAddedId")] Overtime overtime)
		{
			if (ModelState.IsValid)
			{
				if (overtime.Utilized != null)
					overtime.UsrAppliedId = Overtime.GetUserId(User);

				db.Overtimes.Add(overtime);
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { message = "Переработка создана" });
			}

			ViewBag.UsrList = new SelectList(db.Users.OrderBy(u => u.Surname), "Id", "FullName");
			return View(overtime);
		}

		// GET: Overtimes/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Overtime overtime = await db.Overtimes.FindAsync(id);
			if (overtime == null)
			{
				return HttpNotFound();
			}

			return View(overtime);
		}

		// POST: Overtimes/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,UsrId,Date,Duration,Utilized,UsrAddedId")] Overtime overtime)
		{
			if (ModelState.IsValid)
			{
				if (overtime.Utilized != null)
					overtime.UsrAppliedId = Overtime.GetUserId(User);

				db.Entry(overtime).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { message = "Переработка изменена" });
			}

			return View(overtime);
		}

		// GET: Overtimes/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Overtime overtime = await db.Overtimes.FindAsync(id);
			if (overtime == null)
			{
				return HttpNotFound();
			}
			return View(overtime);
		}

		// POST: Overtimes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Overtime overtime = await db.Overtimes.FindAsync(id);
			db.Overtimes.Remove(overtime);
			await db.SaveChangesAsync();
			return RedirectToAction("Index", new { message = "Переработка удалена" });
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
