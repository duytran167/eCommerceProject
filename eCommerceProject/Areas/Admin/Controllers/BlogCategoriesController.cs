using eCommerceProject.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class BlogCategoriesController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: BlogCategories
		public ActionResult Index()
		{
			return View(db.BlogCategories.ToList());
		}

		// GET: BlogCategories/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			BlogCategory blogCategory = db.BlogCategories.Find(id);
			if (blogCategory == null)
			{
				return HttpNotFound();
			}
			return View(blogCategory);
		}

		// GET: BlogCategories/Create

		public ActionResult Create()
		{
			return View();
		}

		// POST: BlogCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,CategoryName")] BlogCategory blogCategory)
		{
			if (ModelState.IsValid)
			{
				db.BlogCategories.Add(blogCategory);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Index");

			}
			return View(blogCategory);
		}

		[HttpGet]
		// GET: BlogCategories/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			BlogCategory blogCategory = db.BlogCategories.Find(id);
			if (blogCategory == null)
			{
				return HttpNotFound();
			}
			return View(blogCategory);
		}

		// POST: BlogCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,CategoryName")] BlogCategory blogCategory)
		{
			if (ModelState.IsValid)
			{
				var post = db.BlogCategories.FirstOrDefault(t => t.Id == blogCategory.Id);
				post.CategoryName = blogCategory.CategoryName;
				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Index");
			}

			return View(blogCategory);
		}

		// GET: BlogCategories/Delete/5
		public ActionResult Delete(int id)
		{
			var removeCategory = db.BlogCategories.SingleOrDefault(t => t.Id == id);
			db.BlogCategories.Remove(removeCategory);
			db.SaveChanges();
			TempData["error"] = "Delete Successfully!";
			return RedirectToAction("Index");
		}
		//public ActionResult Delete(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	BlogCategory blogCategory = db.BlogCategories.Find(id);
		//	if (blogCategory == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(blogCategory);
		//}

		//// POST: BlogCategories/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(int id)
		//{
		//	BlogCategory blogCategory = db.BlogCategories.Find(id);
		//	db.BlogCategories.Remove(blogCategory);
		//	db.SaveChanges();
		//	return RedirectToAction("Index", "BlogCategories");
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
