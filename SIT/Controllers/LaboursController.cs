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
using System.Globalization;

namespace SIT.Controllers
{
	public class LaboursController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Labours
		public ActionResult Index()
		{
			return View(new LabourViewModel());
		}


		// GET: Labours/Create
		public ActionResult Create()
		{
			ViewBag.Users = db.Users.Where(u => u.ParticipateInLabour == true).OrderBy(us => us.Surname).ToList();
			return View();
		}

		// POST: Labours/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Date")] Labour labour, string[] participants)
		{
			if (participants is null)
				ModelState.AddModelError("", "Необходимо выбрать хотя бы одного участника");

			if (ModelState.IsValid)
			{
				labour.Users = new List<ApplicationUser>();
				foreach (var item in participants)
					labour.Users.Add(db.Users.FirstOrDefault(u => u.Id == item));

				db.Labours.Add(labour);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.Users = db.Users.Where(u => u.ParticipateInLabour == true).OrderBy(us => us.Surname).ToList();
			return View(labour);
		}


		// GET: Labours/Delete/5
		public async Task<ActionResult> Delete(string userId, string date)
		{
			if (userId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var dt = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
			var labour = await db.Labours.FirstOrDefaultAsync(l => l.Date == dt && l.Users.Any(u => u.Id == userId));

			var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

			return View(new LabourDeleteModel(labour, user));
		}

		// POST: Labours/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int labourId, string userId)
		{
			var labour = await db.Labours.FindAsync(labourId);
			if (labour.Users.Count == 1)
			{
				db.Labours.Remove(labour);
			}
			else if (labour.Users.Count > 1)
			{
				var usr = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
				labour.Users.Remove(usr);
				db.Entry(labour).State = EntityState.Modified;
			}

			await db.SaveChangesAsync();
			return RedirectToAction("Index");
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
