﻿using System;
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
	public class UnitsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Units
		public async Task<ActionResult> Index()
		{
			var units = db.Units.Include(u => u.Chief);
			ViewBag.Sections = await db.Sections.ToListAsync();
			return View(await units.ToListAsync());
		}

		// GET: Units/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = await db.Units.FindAsync(id);
			if (unit == null)
			{
				return HttpNotFound();
			}
			unit.Chief = db.Users.FirstOrDefault(u => u.Id == unit.ChiefId);
			ViewBag.sections = db.Sections.Where(s => s.UnitId == id).ToList();
			return View(unit);
		}

		// GET: Units/Create
		public ActionResult Create()
		{
			ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList();
			return View();
		}

		// POST: Units/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,ChiefId")] Unit unit)
		{
			if (ModelState.IsValid)
			{
				db.Units.Add(unit);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.ChiefId = new SelectList(db.Users, "Id", "FullName", unit.ChiefId);
			return View(unit);
		}

		// GET: Units/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = await db.Units.FindAsync(id);
			if (unit == null)
			{
				return HttpNotFound();
			}

			return View(unit);
		}

		// POST: Units/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ChiefId")] Unit unit)
		{
			if (ModelState.IsValid)
			{
				db.Entry(unit).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(unit);
		}

		// GET: Units/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = await db.Units.FindAsync(id);
			if (unit == null)
			{
				return HttpNotFound();
			}
			unit.Chief = db.Users.FirstOrDefault(u => u.Id == unit.ChiefId);
			ViewBag.SectionCount = db.Sections.Where(s => s.UnitId == id).Count();
			return View(unit);
		}

		// POST: Units/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Unit unit = await db.Units.FindAsync(id);

			db.Units.Remove(unit);
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
