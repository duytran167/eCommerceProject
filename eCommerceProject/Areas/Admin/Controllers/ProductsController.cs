using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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
			var post = new ProductVM()
			{
				Categories = db.Categories.ToList(),
			};

			return View(post);
		}

		// POST: Admin/Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(ProductVM model, List<HttpPostedFileBase> uploadFile)
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



					var newProduct = new Product()
					{
						ProductName = model.Product.ProductName,
						CategoryID = model.Id,
						ShortDesc = model.Product.ShortDesc,
						Description = model.Product.Description,
						ImagePath = filePath,
						Active = model.Product.Active,
						BestSellers = model.Product.BestSellers,
						CreatedDate = model.Product.CreatedDate,
						Discount = model.Product.Discount,
						Price = model.Product.Price,
						UnitsInStock = model.Product.UnitsInStock,

					};


					db.Products.Add(newProduct);
					db.SaveChanges();
					TempData["success"] = "Create Success!";
					return RedirectToAction("Index");
				}

			}

			return View(model);
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
