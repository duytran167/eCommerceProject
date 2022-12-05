using eCommerceProject.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace eCommerceProject.Areas.Admin.Controllers
{
	public class CollectionsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Collections
		public ActionResult Index()
		{
			return View(db.Collections.ToList().OrderByDescending(t => t.ArticleDate));
		}

		// GET: Admin/Collections/Details/5
		public ActionResult Details(int? id, string search, int? page)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var collections = from s in db.Products.Where(t => t.CollectionID == id).Include(t => t.Collection).OrderByDescending(t => t.CreatedDate).ToList() select s;
			if (!String.IsNullOrWhiteSpace(search))
			{
				collections = collections.Where(s => s.ProductName.Contains(search)
															 || s.ProductCode.Contains(search)).ToList();
			}
			if (collections == null)
			{
				return HttpNotFound();
			}
			return View(collections.ToPagedList(page ?? 1, 20));
		}

		// GET: Admin/Collections/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/Collections/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,ArticleDate,ImageFile")] Collection banner)
		{
			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(banner.ImageFile.FileName);
				string exe = Path.GetExtension(banner.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				banner.ImagePath = "~/Content/ImageProduct/Banner/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Banner/"), fileName);
				banner.ImageFile.SaveAs(fileName);

				var newBanner = new Collection()
				{
					Name = banner.Name,
					Description = banner.Description,
					ArticleDate = DateTime.Now,
					ImagePath = banner.ImagePath,
				};
				db.Collections.Add(newBanner);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Index");
			}
			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		.SelectMany(E => E.Errors)
		.Select(E => E.ErrorMessage)
		.ToList();

			return View(banner);

		}

		// GET: Admin/Collections/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var collection = db.Collections
							 .SingleOrDefault(t => t.Id == id);
			var updateCollection = new Collection()
			{
				Name = collection.Name,
				Description = collection.Description,
				ImagePath = collection.ImagePath,



			};

			return View(updateCollection);
			if (collection == null)
			{
				return HttpNotFound();
			}
			return View(collection);
		}

		// POST: Admin/Collections/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit(
		[Bind(Include = "Id,Name,Description,ImageFile")] Collection banner, HttpPostedFileBase fileImage)
		{
			if (ModelState.IsValid)
			{
				if (fileImage != null && fileImage.ContentLength > 0)
				{
					var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
					var fileName = Path.GetFileNameWithoutExtension(fileImage.FileName);
					string exe = Path.GetExtension(fileImage.FileName);
					fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
					banner.ImagePath = "~/Content/ImageProduct/Banner/" + fileName;
					fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Banner/"), fileName);
					fileImage.SaveAs(fileName);

					var post = db.Collections.FirstOrDefault(t => t.Id == banner.Id);
					post.Name = banner.Name;
					post.Description = banner.Description;
					post.ImagePath = banner.ImagePath;


					db.SaveChanges();
					TempData["success"] = "Update Successfully";
					return RedirectToAction("Index", "Collections");
				}
				else
				{
					var post = db.Collections.FirstOrDefault(t => t.Id == banner.Id);
					post.Name = banner.Name;
					post.Description = banner.Description;



					db.SaveChanges();
					TempData["success"] = "Update Successfully";
					return RedirectToAction("Index", "Collections");
				}
			}
			return View(banner);
		}


		// GET: Admin/Collections/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Collection collection = db.Collections.Find(id);
			db.Collections.Remove(collection);
			db.SaveChanges();
			if (collection == null)
			{
				return HttpNotFound();
			}
			return View(collection);
		}

		// POST: Admin/Collections/Delete/5


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
