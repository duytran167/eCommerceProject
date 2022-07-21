using eCommerceProject.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace eCommerceProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SellerController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		private ApplicationDbContext _context;
		public ApplicationDbContext context
					 => _context ?? (_context = ApplicationDbContext.Create());

		// GET: Seller
		public ActionResult Index(int? page, string searchString, int? filter)
		{

			var sellers = from s in db.Sellers.AsNoTracking().ToList()
										select s;

			if (!String.IsNullOrWhiteSpace(searchString))
			{
				sellers = db.Sellers.Where(s => s.FullName.Contains(searchString)
															 || s.PhoneNumber.Contains(searchString)).ToList();
			}

			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				sellers = sellers.Where(x => x.StatusID.Equals(filter)).ToList();


			}

			return View(sellers.ToPagedList(page ?? 1, 2));
		}


		// GET: Seller/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Seller seller = db.Sellers.Find(id);
			if (seller == null)
			{
				return HttpNotFound();
			}
			return View(seller);
		}

		// GET: Seller/Create


		// GET: Seller/Edit/5
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Seller seller = db.Sellers.Find(id);

			if (seller == null)
			{
				return HttpNotFound();
			}

			var sellers = new Seller()
			{
				Id = seller.Id,
				StatusID = seller.StatusID,
				FullName = seller.FullName,
				Address = seller.Address,
				Email = seller.Email,
				PhoneNumber = seller.PhoneNumber
			};
			return View(sellers);
		}

		// POST: Seller/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Address,StatusID")] Seller seller)
		{
			if (ModelState.IsValid)
			{
				var post = db.Sellers.FirstOrDefault(t => t.Id == seller.Id);
				post.StatusID = seller.StatusID;
				post.FullName = seller.FullName;
				post.Address = seller.Address;
				post.PhoneNumber = seller.PhoneNumber;
				post.Email = seller.Email;
				db.Entry(post).State = EntityState.Modified;
				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Index");
			}
			return View(seller);
		}

		// GET: Seller/Delete/5
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Seller seller = db.Sellers.Find(id);
			if (seller == null)
			{
				return HttpNotFound();
			}
			return View(seller);
		}

		// POST: Seller/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			Seller seller = db.Sellers.Find(id);
			db.Users.Remove(seller);
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
