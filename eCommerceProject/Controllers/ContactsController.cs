using eCommerceProject.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	public class ContactsController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Contacts
		public ActionResult Index()
		{
			return View(db.Contacts.ToList());
		}

		// GET: Contacts/Details/5


		// GET: Contacts/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Contacts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,FullName,PhoneNumber,Email,Content,ArticleDate,ImageFile")] Contact contact)
		{
			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(contact.ImageFile.FileName);
				string exe = Path.GetExtension(contact.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				contact.ImagePath = "~/Content/ImageProduct/Banner/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Banner/"), fileName);
				contact.ImageFile.SaveAs(fileName);

				var newContact = new Contact()
				{
					FullName = contact.FullName,
					PhoneNumber = contact.PhoneNumber,
					Email = contact.Email,
					Content = contact.Content,
					ArticleDate = DateTime.Now,
					ImagePath = contact.ImagePath,
					Noti = false
				};
				db.Contacts.Add(newContact);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Create");
			}
			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		.SelectMany(E => E.Errors)
		.Select(E => E.ErrorMessage)
		.ToList();
			return View(contact);
		}

		// GET: Contacts/Edit/5


		// GET: Contacts/Delete/5
		//public ActionResult Delete(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	Contact contact = db.Contacts.Find(id);
		//	if (contact == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(contact);
		//}

		//// POST: Contacts/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(int id)
		//{
		//	Contact contact = db.Contacts.Find(id);
		//	db.Contacts.Remove(contact);
		//	db.SaveChanges();
		//	return RedirectToAction("Index");
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
