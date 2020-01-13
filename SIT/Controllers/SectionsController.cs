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
	[Authorize(Roles = "manager, admin, chief")]
	public class SectionsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Sections
		public async Task<ActionResult> Index()
		{
			var sections = db.Sections.ToList();
			foreach (var item in sections)
			{
				item.Staff = db.Users.Where(u => u.SectionId == item.Id).ToList();
			}

			return View(sections);
		}


		/// <summary>
		/// добавляет выбранному в ~/Unit/Edit бюро UnitId
		/// </summary>
		/// <param name="sectionId">ид бюро</param>
		/// <param name="unitId">ид отдела</param>
		public void Append(int sectionId, int unitId)
		{
			var section = db.Sections.Find(sectionId);
			section.UnitId = unitId;
			db.SaveChanges();
		}

		/// <summary>
		/// удаляет у выбранного в ~/Unit/Edit бюро UnitId
		/// </summary>
		/// <param name="sectionId">ид бюро</param>
		/// <param name="unitId">ид отдела</param>
		public void Remove(int sectionId, int unitId)
		{
			var section = db.Sections.Find(sectionId);
			if (section.UnitId == unitId)
				section.UnitId = null;
			db.SaveChanges();
		}

		/// <summary>
		/// удаляет у всех сотрудников бюро UnitId
		/// </summary>
		/// <param name="unitId">ид отдела</param>
		public void ClearUnit(int unitId)
		{
			foreach (var section in db.Sections)
			{
				if (section.UnitId == unitId)
					section.UnitId = null;
			}
			db.SaveChanges();
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
			section.Staff = await db.Users.Where(u => u.SectionId == id).ToListAsync();
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

			var section = await db.Sections.FindAsync(id);
			section.Staff = await db.Users.Where(u => u.SectionId == id).OrderBy(u => u.Surname).ToListAsync();

			if (section == null)
			{
				return HttpNotFound();
			}
			ViewBag.Users = await db.Users.ToListAsync();
			return View(section);
		}

		// POST: Sections/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ChiefId,UnitId")] Section section)
		{
			if (ModelState.IsValid)
			{
				db.Entry(section).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.Users = await db.Users.ToListAsync();
			return View(section);
		}

		// GET: Sections/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var section = await db.Sections.FindAsync(id);
			section.Staff = await db.Users.Where(u => u.SectionId == id).OrderBy(u => u.Surname).ToListAsync();

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
			var section = await db.Sections.FindAsync(id);

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
