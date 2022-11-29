﻿using eCommerceProject.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Seller")]
	public class ContactController : Controller
	{
		// GET: Admin/Contact
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Contacts
		public ActionResult Index()
		{
			return View(db.Contacts.OrderByDescending(t => t.ArticleDate).ToList());
		}

		// GET: Contacts/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Contact contact = db.Contacts.Find(id);
			contact.Noti = true;
			db.SaveChanges();
			if (contact == null)
			{
				return HttpNotFound();
			}
			return View(contact);
		}

		// GET: Contacts/Create





		//GET: Contacts/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Contact contact = db.Contacts.Find(id);
			if (contact == null)
			{
				return HttpNotFound();
			}
			return View(contact);
		}
		public ActionResult CountNoti()
		{
			var noti = db.Contacts.Where(t => t.Noti == false).ToList();
			ViewBag.CountNoti = noti.Count();
			return View();
		}

		// POST: Contacts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Contact contact = db.Contacts.Find(id);
			db.Contacts.Remove(contact);
			db.SaveChanges();
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