using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using PagedList;
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
		public ActionResult Index(int? page, string search, int? filter)
		{
			var products = from s in db.Products.Include(p => p.Categories).AsNoTracking().OrderByDescending(t => t.CreatedDate).ToList()
										 select s;
			if (!String.IsNullOrWhiteSpace(search))
			{
				products = products.Where(s => s.ProductName.Contains(search)
															 || s.ProductCode.Contains(search)).ToList();
			}
			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				products = products.Where(x => x.CategoriesID.Equals(filter)).ToList();

			}



			ViewBag.product = db.Products.ToList();
			ViewData["DanhMuc"] = new SelectList(db.Categories, "Id", "CategoryName", "ImagePath");
			return View(products.ToList());
		}

		[HttpPost]
		public JsonResult Index(int? page, string search, int? filter, string sortName, string sortDirection)
		{
			var products = from s in db.Products.Include(p => p.Categories).AsNoTracking().OrderByDescending(t => t.CreatedDate).ToList()
										 select s;
			if (!String.IsNullOrWhiteSpace(search))
			{
				products = products.Where(s => s.ProductName.Contains(search)
															 || s.ProductCode.Contains(search)).ToList();
			}
			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				products = products.Where(x => x.CategoriesID.Equals(filter)).ToList();

			}
			//sort name
			switch (sortName)
			{
				case "Id":
				case "":
					if (sortDirection == "ASC")
					{
						products = (from product in db.Products
												select product)
						.OrderBy(product => product.Id)
						//.Skip(startIndex)
						.ToPagedList(page ?? 1, 5);
					}
					else
					{
						products = (from product in db.Products
												select product)
						.OrderByDescending(product => product.Id)
						//.Skip(startIndex)
						.ToPagedList(page ?? 1, 5);
					}
					break;
				case "ProductName":
					if (sortDirection == "ASC")
					{
						products = (from product in db.Products
												select product)
						.OrderBy(product => product.ProductName)
						//.Skip(startIndex)
						.ToPagedList(page ?? 1, 5);
					}
					else
					{
						products = (from product in db.Products
												select product)
						.OrderByDescending(product => product.ProductName)
						//.Skip(startIndex)
						.ToPagedList(page ?? 1, 5);
					}
					break;
					//case "City":
					//	if (sortDirection == "ASC")
					//	{
					//		model.Customers = (from customer in entities.Customers
					//											 select customer)
					//		.OrderBy(customer => customer.City)
					//		.Skip(startIndex)
					//		.Take(model.PageSize).ToList();
					//	}
					//	else
					//	{
					//		model.Customers = (from customer in entities.Customers
					//											 select customer)
					//	.OrderByDescending(customer => customer.City)
					//	.Skip(startIndex)
					//	.Take(model.PageSize).ToList();
					//	}
					//	break;
					//case "Country":
					//	if (sortDirection == "ASC")
					//	{
					//		model.Customers = (from customer in entities.Customers
					//											 select customer)
					//		.OrderBy(customer => customer.Country)
					//		.Skip(startIndex)
					//		.Take(model.PageSize).ToList();
					//	}
					//	else
					//	{
					//		model.Customers = (from customer in entities.Customers
					//											 select customer)
					//		.OrderByDescending(customer => customer.Country)
					//		.Skip(startIndex)
					//		.Take(model.PageSize).ToList();
					//	}
					break;
			}
			ViewBag.product = db.Products.ToList();
			ViewData["DanhMuc"] = new SelectList(db.Categories, "Id", "CategoryName", "ImagePath");
			return Json(products.ToList());
		}


		// GET: Admin/Products/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			var product = new ViewModel.ProductVM();
			product.Product = db.Products.
							Include(i => i.Categories).
						SingleOrDefault(t => t.Id == id);

			if (product == null)
			{
				return RedirectToAction("Error", "Admin");
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

			ViewBag.Categories = new SelectList(db.Categories, "Id", "CategoryName", product.Categories);
			return View(product);
		}
		// POST: Admin/Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]

		public ActionResult Create(ViewModel.ProductVM model)
		{

			model.Categories = db.Categories.ToList();

			if (ModelState.IsValid)
			{

				//string fileName = Path.GetFileNameWithoutExtension(model.Product.ImageFile.FileName);

				//string exe = Path.GetExtension(model.Product.ImageFile.FileName);
				//fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				//model.Product.ImagePath = "~/Content/ImageProduct/ImageBlog/" + fileName;
				//fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileName);
				//model.Product.ImageFile.SaveAs(fileName);

				List<ImageProduct> fileDetails = new List<ImageProduct>();
				for (int i = 0; i < Request.Files.Count; i++)
				{
					var file = Request.Files[i];

					if (file != null && file.ContentLength > 0)
					{
						var fileName = Path.GetFileName(file.FileName);
						ImageProduct fileDetail = new ImageProduct()
						{
							FileName = "~/Content/ImageProduct/" + fileName,
							Extension = Path.GetExtension(fileName),
							Id = Guid.NewGuid()
						};
						fileDetails.Add(fileDetail);

						var path = Path.Combine(Server.MapPath("~/Content/ImageProduct/"), fileDetail.Id + fileDetail.Extension);

						file.SaveAs(path);
					}
				}

				try
				{
					var newProduct = new Product()
					{
						ProductName = model.Product.ProductName,
						CategoriesID = model.Id,
						ShortDesc = model.Product.ShortDesc,
						Description = model.Product.Description,
						ImageProducts = fileDetails,
						Active = model.Product.Active,
						BestSellers = model.Product.BestSellers,
						CreatedDate = model.Product.CreatedDate,
						Discount = model.Product.Discount,
						Price = model.Product.Price,


					};




					db.Products.Add(newProduct);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					ViewBag.Msg = ex.Message;
				}
			}
			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		.SelectMany(E => E.Errors)
		.Select(E => E.ErrorMessage)
		.ToList();
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
			ViewBag.CategoryID = new SelectList(db.Categories, "Id", "CategoryName", product.Categories);
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
			ViewBag.CategoryID = new SelectList(db.Categories, "Id", "CategoryName", product.Categories);
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