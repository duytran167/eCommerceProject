using eCommerceProject.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	public class CatsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Cats
		public ActionResult Index()
		{
			return View(db.Cat.ToList());
		}

		// GET: Cats/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Cat cat = db.Cat.Find(id);
			if (cat == null)
			{
				return HttpNotFound();
			}
			return View(cat);
		}

		// GET: Cats/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Cats/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(Cat model, List<HttpPostedFileBase> uploadFile)
		{
			if (ModelState.IsValid)
			{
				string abc = "";
				string def = "";
				foreach (var item in uploadFile)
				{

					string filePath = Path.Combine(HttpContext.Server.MapPath("~/Content/ImageProduct/ImageBlog/"),
																				 Path.GetFileName(item.FileName));
					item.SaveAs(filePath);

					abc = string.Format("Upload {0} file thành công", uploadFile.Count);

					def += item.FileName + "; ";



					var newProduct = new Cat()
					{
						ImagePath = filePath,


					};


					db.Cat.Add(newProduct);
					db.SaveChanges();
					TempData["success"] = "Create Success!";
					return RedirectToAction("Index");
				}

			}

			return View(model);
		}

		// GET: Cats/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Cat cat = db.Cat.Find(id);
			if (cat == null)
			{
				return HttpNotFound();
			}
			return View(cat);
		}

		// POST: Cats/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,ImagePath")] Cat cat)
		{
			if (ModelState.IsValid)
			{
				db.Entry(cat).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(cat);
		}

		// GET: Cats/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Cat cat = db.Cat.Find(id);
			if (cat == null)
			{
				return HttpNotFound();
			}
			return View(cat);
		}

		// POST: Cats/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Cat cat = db.Cat.Find(id);
			db.Cat.Remove(cat);
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
