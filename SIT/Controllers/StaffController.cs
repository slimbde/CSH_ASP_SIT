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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace SIT.Controllers
{
	[Authorize(Roles = "manager, admin, chief")]
	public class StaffController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Staff
		public async Task<ActionResult> Index()
		{
			var unit = VotingEngine.GetUserUnit(User);
			IQueryable<ApplicationUser> users;
			if (User.IsInRole("manager"))
			{
				users = db.Users.Where(u => u.Section.UnitId == unit || u.Id == (db.Units.FirstOrDefault(un => un.Id == unit).ChiefId));
				return View(await users.ToListAsync());
			}
			else if (User.IsInRole("chief"))
			{
				var userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
				var sectionId = db.Sections.FirstOrDefault(s => s.ChiefId == userId).Id;
				users = db.Users.Where(u => u.SectionId == sectionId);
				return View(await users.ToListAsync());
			}

			return View(await db.Users.ToListAsync());
		}

		/// <summary>
		/// добавляет выбранному в ~/Section/Edit сотруднику SectionId
		/// </summary>
		/// <param name="id">ид сотрудника</param>
		/// <param name="section">ид секции</param>
		public void Append(string id, int section)
		{
			var user = db.Users.Find(id);
			user.SectionId = section;
			db.SaveChanges();
		}

		/// <summary>
		/// удаляет у выбранного в ~/Section/Edit сотрудника SectionId
		/// </summary>
		/// <param name="id">ид сотрудника</param>
		/// <param name="section">ид секции</param>
		public void Remove(string id, int section)
		{
			var user = db.Users.Find(id);
			if (user.SectionId == section)
				user.SectionId = null;

			db.SaveChanges();
		}

		/// <summary>
		/// удаляет у всех сотрудников секции SectionId
		/// </summary>
		/// <param name="section">ид секции</param>
		public void ClearSection(int section)
		{
			foreach (var user in db.Users)
			{
				if (user.SectionId == section)
					user.SectionId = null;
			}

			db.SaveChanges();
		}

		// GET: Staff/Details/5
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var applicationUser = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		//// GET: Staff/Create
		//public ActionResult Create()
		//{
		//	return View();
		//}

		//// POST: Staff/Create
		//// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		//// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Create([Bind(Include = "Id,Name,Surname,Patronic,TabNo,SectionId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.Users.Add(applicationUser);

		//		await db.SaveChangesAsync();
		//		return RedirectToAction("Index");
		//	}

		//	return View(applicationUser);
		//}

		// GET: Staff/Edit/5
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}

			ViewBag.Sections = await db.Sections.ToListAsync();
			return View(applicationUser);
		}

		// POST: Staff/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Patronic,TabNo,SectionId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,ParticipateInLabour")] ApplicationUser applicationUser, string role = "")
		{
			if (ModelState.IsValid)
			{
				db.Entry(applicationUser).State = EntityState.Modified;
				await db.SaveChangesAsync();

				// изменение роли пользователя
				if (role != "")
				{
					var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

					userManager.RemoveFromRole(applicationUser.Id, "admin");
					userManager.RemoveFromRole(applicationUser.Id, "manager");
					userManager.RemoveFromRole(applicationUser.Id, "chief");

					if (role != "none")
					{
						if (role == "manager" || role == "admin")
						{
							applicationUser.SectionId = null;
							await db.SaveChangesAsync();
						}

						userManager.AddToRole(applicationUser.Id, role);
					}
				}

				return RedirectToAction("Index");
			}
			ViewBag.Sections = await db.Sections.ToListAsync();
			return View(applicationUser);
		}

		// GET: Staff/Delete/5
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}

			return View(applicationUser);
		}

		// POST: Staff/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			db.Users.Remove(applicationUser);
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
