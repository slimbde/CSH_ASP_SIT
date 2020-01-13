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
		public ActionResult Index(int? unit, int? section, string usrId, string year, int? duration, string available)
		{
			bool notUsed = available != null ? true : false;
			var model = new OvertimeListViewModel(User, unit, section, usrId, year, duration, notUsed);

			return View(model);
		}

		//// GET: Overtimes/Details/5
		//public async Task<ActionResult> Details(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Overtime overtime = await db.Overtimes.FindAsync(id);
		//    if (overtime == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(overtime);
		//}

		//// GET: Overtimes/Create
		//public ActionResult Create()
		//{
		//    ViewBag.UsrId = new SelectList(db.ApplicationUsers, "Id", "Name");
		//    return View();
		//}

		//// POST: Overtimes/Create
		//// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		//// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Create([Bind(Include = "Id,UsrId,Date,Duration,Utilized")] Overtime overtime)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Overtimes.Add(overtime);
		//        await db.SaveChangesAsync();
		//        return RedirectToAction("Index");
		//    }

		//    ViewBag.UsrId = new SelectList(db.ApplicationUsers, "Id", "Name", overtime.UsrId);
		//    return View(overtime);
		//}

		//// GET: Overtimes/Edit/5
		//public async Task<ActionResult> Edit(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Overtime overtime = await db.Overtimes.FindAsync(id);
		//    if (overtime == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    ViewBag.UsrId = new SelectList(db.ApplicationUsers, "Id", "Name", overtime.UsrId);
		//    return View(overtime);
		//}

		//// POST: Overtimes/Edit/5
		//// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		//// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Edit([Bind(Include = "Id,UsrId,Date,Duration,Utilized")] Overtime overtime)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Entry(overtime).State = EntityState.Modified;
		//        await db.SaveChangesAsync();
		//        return RedirectToAction("Index");
		//    }
		//    ViewBag.UsrId = new SelectList(db.ApplicationUsers, "Id", "Name", overtime.UsrId);
		//    return View(overtime);
		//}

		//// GET: Overtimes/Delete/5
		//public async Task<ActionResult> Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Overtime overtime = await db.Overtimes.FindAsync(id);
		//    if (overtime == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(overtime);
		//}

		//// POST: Overtimes/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> DeleteConfirmed(int id)
		//{
		//    Overtime overtime = await db.Overtimes.FindAsync(id);
		//    db.Overtimes.Remove(overtime);
		//    await db.SaveChangesAsync();
		//    return RedirectToAction("Index");
		//}

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
