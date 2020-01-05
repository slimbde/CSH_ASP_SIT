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
	public class SectionsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Sections
		public async Task<ActionResult> Index()
		{
			var sections = db.Sections.Include(s => s.Chief);
			return View(await sections.OrderBy(s => s.Name).ToListAsync());
		}

		// GET: Sections/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Section section = await db.Sections.FindAsync(id);
			if (section == null)
			{
				return HttpNotFound();
			}
			section.Workers = await db.Users.Where(u => u.SectionId == id).ToListAsync();
			return View(section);
		}

		// GET: Sections/Create
		public ActionResult Create()
		{
			ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList();
			return View();
		}

		// POST: Sections/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,ChiefId")] Section section)
		{
			if (ModelState.IsValid)
			{
				db.Users.FirstOrDefault(u => u.Id == section.ChiefId).SectionId = section.Id;
				db.Sections.Add(section);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(section);
		}

		// GET: Sections/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Section section = await db.Sections.FindAsync(id);
			if (section == null)
			{
				return HttpNotFound();
			}
			ViewBag.Users = db.Users.OrderBy(u => u.Surname).ToList();
			return View(section);
		}

		// POST: Sections/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ChiefId")] Section section)
		{
			if (ModelState.IsValid)
			{
				db.Entry(section).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(section);
		}

		// GET: Sections/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Section section = await db.Sections.FindAsync(id);
			if (section == null)
			{
				return HttpNotFound();
			}
			return View(section);
		}

		// POST: Sections/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var section = db.Sections.Find(id);

			db.Sections.Remove(section);
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
