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
		public async Task<ActionResult> Index()
		{
			return View(await db.Vacations.ToListAsync());
		}

		// GET: Vacations/Details/5
		public async Task<ActionResult> Details(int? id)
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

		// GET: Vacations/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Vacations/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,StartDate,EndDate,Duration")] Vacation vacation)
		{
			if (ModelState.IsValid)
			{
				db.Vacations.Add(vacation);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

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
		public async Task<ActionResult> Edit([Bind(Include = "Id,StartDate,EndDate,Duration")] Vacation vacation)
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
