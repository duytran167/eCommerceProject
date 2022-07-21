using eCommerceProject.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	[Authorize(Roles = "Admin,Seller")]
	public class AdminCustomersController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: AdminCustomers

		public ActionResult Index(int? page, string searchString, int? filter)
		{
			//view all itemn in page
			var totalItem = db.Customers.ToList().Count();
			ViewBag.totalItem = totalItem;
			// get seller db
			var customers = from s in db.Customers.AsNoTracking().ToList()
											select s;

			if (!String.IsNullOrWhiteSpace(searchString))
			{
				customers = db.Customers.Where(s => s.FullName.Contains(searchString)
															 || s.PhoneNumber.Contains(searchString)).ToList();
			}

			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				customers = customers.Where(x => x.StatusID.Equals(filter)).ToList();


			}

			return View(customers.ToPagedList(page ?? 1, 2));
		}

		// GET: AdminCustomers/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Customer user = db.Customers.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// GET: AdminCustomers/Create


		// GET: AdminCustomers/Edit/5
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Customer customer = db.Customers.Find(id);

			if (customer == null)
			{
				return HttpNotFound();
			}

			var customers = new Customer()
			{
				Id = customer.Id,
				StatusID = customer.StatusID,
				FullName = customer.FullName,
				Address = customer.Address,
				Email = customer.Email,
				PhoneNumber = customer.PhoneNumber
			};
			return View(customers);
		}

		// POST: AdminCustomers/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Address,StatusID")] Customer user)
		{
			if (ModelState.IsValid)
			{
				var post = db.Customers.FirstOrDefault(t => t.Id == user.Id);
				post.StatusID = user.StatusID;
				post.FullName = user.FullName;
				post.Address = user.Address;
				post.PhoneNumber = user.PhoneNumber;
				post.Email = user.Email;
				db.Entry(post).State = EntityState.Modified;
				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Index");
			}
			return View(user);
		}

		// GET: AdminCustomers/Delete/5
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Customer user = db.Customers.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: AdminCustomers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			Customer user = db.Customers.Find(id);
			db.Users.Remove(user);
			db.SaveChanges();
			TempData["success"] = "Delete Success!";
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
