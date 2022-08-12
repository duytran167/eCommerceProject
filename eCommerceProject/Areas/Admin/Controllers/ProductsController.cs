using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	public class ProductsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Products
		public ActionResult Index()
		{
			var products = db.Products.Include(p => p.Category);
			return View(products.ToList());
		}

		// GET: Admin/Products/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		// GET: Admin/Products/Create
		[HttpGet]
		public ActionResult Create()
		{
			var product = new ProductVM()
			{
				Categories = db.Categories.ToList(),
			};
			//IEnumerable<SelectListItem> userTypeList = new SelectList(db.Categories.ToList(), "Id", "CategoryName");
			//ViewData["userTypeList"] = userTypeList;
			////ViewBag.EntidadList = new SelectList(db.Categories.Select(x =>
			////							new
			////							{
			////								Id = x.Id.ToString(),
			////								CategoryName = x.CategoryName
			////							}),
			////					"Id",
			////					"CategoryName");
			ViewBag.EntidadList = new SelectList(db.Categories, "Id", "CategoryName");
			return View(product);
		}
		// POST: Admin/Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]

		public ActionResult Create(ViewModel.ProductVM support)
		{
			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));

				List<ImageProduct> fileDetails = new List<ImageProduct>();
				for (int i = 0; i < Request.Files.Count; i++)
				{
					var file = Request.Files[i];

					if (file != null && file.ContentLength > 0)
					{
						var fileName = Path.GetFileName(file.FileName);
						ImageProduct fileDetail = new ImageProduct()
						{
							FileName = fileName,
							Extension = Path.GetExtension(fileName),
							Id = Guid.NewGuid()
						};
						fileDetails.Add(fileDetail);

						var path = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileDetail.Id + fileDetail.Extension);
						file.SaveAs(path);
					}
				}


				var newProduct = new Product()
				{
					ProductName = support.Product.ProductName,
					CategoryID = 1,
					ShortDesc = support.Product.ShortDesc,
					Description = support.Product.Description,
					ImageProducts = fileDetails,
					Active = support.Product.Active,
					BestSellers = support.Product.BestSellers,
					CreatedDate = support.Product.CreatedDate,
					Discount = support.Product.Discount,
					Price = support.Product.Price,
					UnitsInStock = support.Product.UnitsInStock,

				};




				db.Products.Add(newProduct);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(support);
		}
		// GET: Admin/Products/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryID);
			return View(product);
		}

		// POST: Admin/Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,ProductName,ShortDesc,Description,ImagePath,CategoryID,Price,Discount,UnitsInStock,BestSellers,Active,CreatedDate")] Product product)
		{
			if (ModelState.IsValid)
			{
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryID);
			return View(product);
		}

		// GET: Admin/Products/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		// POST: Admin/Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Product product = db.Products.Find(id);
			db.Products.Remove(product);
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