using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using SIT.Models;

namespace SIT.Controllers
{
	[Authorize]
	public class VacationsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index(int unit = 1)
		{
			if (!User.IsInRole("admin"))
			{
				unit = VotingEngine.GetUserUnit(User);

				// когда пользователь не прописан в бюро или отдел
				if (unit == -1)
				{
					ViewBag.Message = "Пользователь не состоит ни в бюро ни в отделе";
					return View("Error");
				}
			}

			bool voting = VotingEngine.GetVotingStatus(unit);
			var year = voting ? DateTime.Now.Year + 1 : DateTime.Now.Year;
			//var year = 2020; // для проверки
			var ViewModel = new VacationVotingViewModel(year, User, unit, voting);

			ViewBag.ReturnAction = "Index";
			return View(ViewModel);
		}


		/// <summary>
		/// метод для инициализации голосования
		/// </summary>
		[Authorize(Roles = "admin, manager")]
		public ActionResult InitiateVoting(int unit)
		{
			VotingEngine.InitiateVoting(unit);

			return RedirectToAction("Index");
		}


		/// <summary>
		/// метод для остановки голосования
		/// </summary>
		[Authorize(Roles = "admin, manager")]
		public ActionResult CancelVoting(int unit, int year)
		{
			VotingEngine.CancelVoting(unit, year);

			return RedirectToAction("Index");
		}


		[Authorize(Roles = "admin, manager, chief")]
		public ActionResult Review(int? unit, int? section, string UsrId, string Year, string Month)
		{
			var vlvm = new VacationListViewModel(User, unit, section, UsrId, Year, Month);

			return View(vlvm);
		}


		// GET: Vacations/Create
		public ActionResult Create()
		{
			var model = new Vacation { UsrId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id, Duration = 14 };

			var unit = VotingEngine.GetUserUnit(User);

			// когда заходит обычный пользователь для голосования
			if (!User.IsInRole("admin") && !User.IsInRole("chief") && !User.IsInRole("manager"))
			{
				var currentVotingUserName = VotingEngine.GetVotingUserName(unit);
				if (currentVotingUserName == "not started")
					return RedirectToAction("Index");

				var requestUserName = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName;
				if (!currentVotingUserName.Equals(requestUserName))
					return RedirectToAction("Index");

				// ему для выбора сотрудника доступен только он
				ViewBag.UserList = new SelectList(db.Users.Where(u => u.UserName == User.Identity.Name), "Id", "FullName");
				return View(model);
			}
			else if (User.IsInRole("chief"))
			{
				var section = db.Sections.FirstOrDefault(s => s.Chief.UserName == User.Identity.Name).Id;
				ViewBag.UserList = new SelectList(db.Users.Where(u => u.SectionId == section).OrderBy(u => u.Surname), "Id", "FullName");
			}
			else if (User.IsInRole("manager"))
			{
				db.Sections.Include(sec => sec.Unit);
				var unitChief = db.Units.FirstOrDefault(un => un.Id == unit).Chief;
				ViewBag.UserList = new SelectList(db.Users.Where(us => us.Section.Unit.ChiefId == unitChief.Id || us.Id == unitChief.Id).OrderBy(u => u.Surname), "Id", "FullName");
			}
			else
				ViewBag.UserList = new SelectList(db.Users.OrderBy(u => u.Surname), "Id", "FullName");

			return View(model);
		}

		// POST: Vacations/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
		// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,UsrId,Duration")] Vacation vacation, DateTime date, string returnAction)
		{
			// смотрим какие уже заявки есть
			var votes = db.Vacations.Where(v => v.UsrId == vacation.UsrId && v.Year == date.Year).ToList();
			var votedDays = 0;
			if (votes.Count > 0)
				votedDays = votes.Sum(v => v.Duration);

			// проверяем валидность ввода продолжительности
			var availableDays = 28 - votedDays;
			if (vacation.Duration > availableDays)
				ModelState.AddModelError("Duration", $"Неверно введена продолжительность. Вам доступно: {availableDays} дней");

			if (ModelState.IsValid)
			{
				vacation.Year = date.Year;
				vacation.Month = date.Month;
				db.Vacations.Add(vacation);
				db.SaveChanges();

				// если новая сумма достигла предела - выставляем флаг проголосовал
				if (availableDays == vacation.Duration)
				{
					db.Votings.FirstOrDefault(v => v.UsrId == vacation.UsrId).Voted = true;
					db.SaveChanges();
				}

				return RedirectToAction(returnAction);
			}

			ViewBag.UserList = new SelectList(db.Users.OrderBy(u => u.Surname), "Id", "FullName");
			return View(vacation);
		}

		// GET: Vacations/Edit/5
		[Authorize(Roles = "admin, manager, chief")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var vacation = await db.Vacations.Include(v => v.Usr).FirstOrDefaultAsync(v => v.Id == id);
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
		[Authorize(Roles = "admin, manager, chief")]
		public async Task<ActionResult> Edit([Bind(Include = "Id,UsrId,Year,Month,Duration")] Vacation vacation)
		{
			if (ModelState.IsValid)
			{
				db.Entry(vacation).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Review");
			}
			vacation = await db.Vacations.Include(v => v.Usr).FirstOrDefaultAsync(v => v.Id == vacation.Id);
			return View(vacation);
		}

		// GET: Vacations/Delete/5
		[Authorize(Roles = "admin, manager, chief")]
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
		[Authorize(Roles = "admin, manager, chief")]
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
