using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Seller")]
	public class ProductsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Products

		//public ActionResult Index(int? page, string search, int? filter)
		//{
		//	var products = from s in db.Products.Include(p => p.Categories).AsNoTracking().OrderByDescending(t => t.CreatedDate).ToList()
		//								 select s;
		//	if (!String.IsNullOrWhiteSpace(search))
		//	{
		//		products = products.Where(s => s.ProductName.Contains(search)
		//													 || s.ProductCode.Contains(search)).ToList();
		//	}
		//	if (!String.IsNullOrWhiteSpace(filter.ToString()))
		//	{
		//		//Filter results based on company selected.

		//		products = products.Where(x => x.CategoriesID.Equals(filter)).ToList();

		//	}



		//	ViewBag.product = db.Products.ToList();
		//	ViewData["DanhMuc"] = new SelectList(db.Categories, "Id", "CategoryName", "ImagePath");
		//	return View(products.ToList());
		//}

		public ActionResult Index(string Sorting_Order, string search, int? filter, string Search_Data, string Filter_Value, int? Page_No)
		{
			ViewBag.CurrentSortOrder = Sorting_Order;
			ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "ProductName" : "";
			ViewBag.SortingPrice = String.IsNullOrEmpty(Sorting_Order) ? "Price" : "";
			ViewBag.SortingId = String.IsNullOrEmpty(Sorting_Order) ? "Id" : "";
			ViewBag.SortingDate = Sorting_Order == "CreatedDate" ? "ProductName" : "Date";
			ViewData["DanhMuc"] = new SelectList(db.Categories, "Id", "CategoryName", "ImagePath");



			if (Search_Data != null)
			{
				Page_No = 1;
			}
			else
			{
				Search_Data = Filter_Value;
			}

			ViewBag.FilterValue = Search_Data;

			ViewBag.category = db.Categories.ToList();

			var products = from stu in db.Products.Include(t => t.Categories).AsNoTracking()
										 .OrderBy(t => t.CreatedDate)
										 .Include(t => t.ImageProducts)
										 .Include(t => t.Categories).ToList()
										 select stu;
			if (!String.IsNullOrWhiteSpace(search))
			{
				products = products.Where(stu => stu.ProductName.Contains(search)
															 || stu.ProductCode.Contains(search)).ToList();
			}
			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				products = products.Where(stu => stu.CategoriesID.Equals(filter)).ToList();

			}
			if (!String.IsNullOrEmpty(Search_Data))
			{
				products = products.Where(stu => stu.ProductName.ToUpper().Contains(Search_Data.ToUpper())
				|| stu.ProductCode.ToUpper().Contains(Search_Data.ToUpper()));
			}
			switch (Sorting_Order)
			{
				case "ProductName":
					products = products.OrderByDescending(stu => stu.ProductName);
					break;
				case "Id":
					products = products.OrderByDescending(stu => stu.Id);
					break;
				case "Price":
					products = products.OrderByDescending(stu => stu.Price);
					break;
				default:
					products = products.OrderBy(stu => stu.CreatedDate);
					break;
			}

			int Size_Of_Page = 20;
			int No_Of_Page = (Page_No ?? 1);
			return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
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
			//comment product
			ViewBag.ProductId = id;
			var comments = db.StarRatingAndComments.Where(d => d.ProductId.Equals(id.Value)).Include(t => t.User).ToList();
			ViewBag.Comments = comments;

			var ratings = db.StarRatingAndComments.Where(d => d.ProductId.Equals(id.Value)).Include(t => t.User).ToList();
			if (ratings.Count() > 0)
			{
				var ratingSum = ratings.Sum(d => d.Rating);
				ViewBag.RatingSum = ratingSum;
				var ratingCount = ratings.Count();
				ViewBag.RatingCount = ratingCount;
			}
			else
			{
				ViewBag.RatingSum = 0;
				ViewBag.RatingCount = 0;
			}
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
					// get asnd create image
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

						var path = Path.Combine(Server.MapPath("~/Content/ImageProduct/"), fileName);

						file.SaveAs(path);
					}
				}
				//get size
				var sizeVip = model.Size;
				List<Size> sizes = new List<Size>();

				foreach (var item in model.Sizes)
				{
					var sizeV = new Size()

					{
						SizeName = item.SizeName,
						Stock = item.Stock,

					};
					sizes.Add(sizeV);
				}

				try
				{
					var newProduct = new Product()
					{
						ProductName = model.Product.ProductName,
						ProductCode = model.Product.ProductCode,
						CategoriesID = model.Id,
						ShortDesc = model.Product.ShortDesc,
						Description = model.Product.Description,
						ImageProducts = fileDetails,
						Active = model.Product.Active,
						BestSellers = model.Product.BestSellers,
						CreatedDate = model.Product.CreatedDate,
						Discount = model.Product.Discount,
						Price = model.Product.Price,
						PriceSale = model.Product.PriceSale,
						Sizes = sizes

					};

					db.Products.Add(newProduct);
					db.SaveChanges();
					TempData["success"] = "Create Success!";
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
		public ActionResult Edit(int id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			var product = db.Products.SingleOrDefault(t => t.Id == id);


			var pro = new ViewModel.ProductVM()
			{
				Id = id,
				Product = product,
				Categories = db.Categories.ToList(),


			};

			if (product == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "Id", "CategoryName", product.Categories);
			return View(pro);
		}

		// POST: Admin/Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]

		public ActionResult Edit(ViewModel.ProductVM model)
		{
			if (ModelState.IsValid)
			{
				var product = db.Products.SingleOrDefault(t => t.Id == model.Id);

				product.ProductName = model.Product.ProductName;
				product.CategoriesID = model.Product.CategoriesID;
				product.ShortDesc = model.Product.ShortDesc;
				product.Description = model.Product.Description;
				product.ProductCode = model.Product.ProductCode;
				product.Active = model.Product.Active;
				product.BestSellers = model.Product.BestSellers;
				product.PriceSale = model.Product.PriceSale;
				product.Discount = model.Product.Discount;
				product.Price = model.Product.Price;




				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Details", new { id = product.Id });

			}
			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		.SelectMany(E => E.Errors)
		.Select(E => E.ErrorMessage)
		.ToList();
			return View(model);
		}
		//	//get error when model state í false
		//	var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		//.SelectMany(E => E.Errors)
		//.Select(E => E.ErrorMessage)
		//.ToList();
		//	return View(model);
		//}

		// GET: Admin/Products/Delete/5
		public ActionResult Delete(int id)
		{
			var removePro = db.Products.SingleOrDefault(t => t.Id == id);
			db.Products.Remove(removePro);
			db.SaveChanges();

			return RedirectToAction("Index");
			TempData["success"] = "Delete Success!";
		}
		public ActionResult DeleteComment(int? id)
		{

			if (ModelState.IsValid)
			{
				//string currentUserId = User.Identity.GetUserId();
				var removeCmt = db.StarRatingAndComments
					//.Where(x => x.UserID == currentUserId)
					.SingleOrDefault(t => t.Id == id);
				db.StarRatingAndComments.Remove(removeCmt);
				db.SaveChanges();
				return RedirectToAction("Details", "", new { id = removeCmt.ProductId });
			}
			return View(id);
		}

		// for size----------------------------------------------------
		public ActionResult CreateSize()
		{
			var size = new ViewModel.ProductVM()
			{
				Sizes = db.Sizes.ToList(),
			};

			return View(size);
		}

		// POST: BlogCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateSize([Bind(Include = "Id,SizeName,Stock,ProductId")] Size size, int id)
		{
			if (ModelState.IsValid)
			{

				var newSize = new Size()
				{
					SizeName = size.SizeName,
					Stock = size.Stock,
					ProductId = id
				};
				db.Sizes.Add(newSize);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Details", new { id = newSize.ProductId });

			}
			return View(size);
		}
		public ActionResult EditSize(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			Size size = db.Sizes.Find(id);
			if (size == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			return View(size);
		}

		// POST: BlogCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSize([Bind(Include = "Id,SizeName,Stock")] Size size)
		{
			if (ModelState.IsValid)
			{
				var post = db.Sizes.FirstOrDefault(t => t.Id == size.Id);
				post.SizeName = size.SizeName;
				post.Stock = size.Stock;
				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Details", new { id = post.Product.Id });
			}

			return View(size);
		}
		public ActionResult DeleteSize(int id)
		{
			var removeSize = db.Sizes.SingleOrDefault(t => t.Id == id);
			db.Sizes.Remove(removeSize);
			db.SaveChanges();
			TempData["success"] = "Delete Successfully!";
			return RedirectToAction("Details", new { id = removeSize.Product.Id });
		}
		//image crud for product

		////for Filter parital view
		//[ChildActionOnly]
		//public ActionResult EditImage(int? id)
		//{
		//	if (id == null)
		//	{
		//		return RedirectToAction("Error", "Admin");
		//	}
		//	ImageProduct size = db.ImageProducts.Find(id);
		//	if (size == null)
		//	{
		//		return RedirectToAction("Error", "Admin");
		//	}
		//	return PartialView(size);
		//}

		//// POST: BlogCategories/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		//// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult EditImage([Bind(Include = "Id,FileName,Extension")] ImageProduct image, HttpPostedFileBase fileImage)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		if (fileImage != null && fileImage.ContentLength > 0)
		//		{
		//			var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
		//			var fileName = Path.GetFileName(fileImage.FileName);
		//			string exe = Path.GetExtension(fileImage.FileName);
		//			fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
		//			image.FileName = "~/Content/ImageProduct/" + fileName;
		//			fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/"), fileName);
		//			fileImage.SaveAs(fileName);


		//			var post = db.ImageProducts.FirstOrDefault(t => t.Id == image.Id);
		//			post.FileName = image.FileName;

		//			post.Extension = Path.GetExtension(fileName);


		//			db.SaveChanges();
		//			TempData["success"] = "Update Image Successfully";
		//			return RedirectToAction("Details", new { id = post.ProductId });
		//		}

		//	}

		//	return View(image);
		//}
		public ActionResult CreateImage()
		{
			var size = new ViewModel.ProductVM()
			{
				ImageProducts = db.ImageProducts.ToList(),
			};

			return View(size);
		}

		// POST: BlogCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateImage([Bind(Include = "Id,FileName,Extension,ProductId")] ImageProduct image, int id)
		{




			List<ImageProduct> fileDetails = new List<ImageProduct>();
			for (int i = 0; i < Request.Files.Count; i++)
			{
				var file = Request.Files[i];
				// get asnd create image
				if (file != null && file.ContentLength > 0)
				{
					var fileName = Path.GetFileName(file.FileName);
					ImageProduct fileDetail = new ImageProduct()
					{
						FileName = "~/Content/ImageProduct/" + fileName,
						Extension = Path.GetExtension(fileName),
						Id = Guid.NewGuid(),
						ProductId = id
					};
					fileDetails.Add(fileDetail);

					var path = Path.Combine(Server.MapPath("~/Content/ImageProduct/"), fileName);

					file.SaveAs(path);
					db.ImageProducts.Add(fileDetail);

					db.SaveChanges();
					TempData["success"] = "Create Success!";
					return RedirectToAction("Details", new { id = fileDetail.ProductId });

				}
			}

			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
		.SelectMany(E => E.Errors)
		.Select(E => E.ErrorMessage)
		.ToList();
			return View(image);
		}
		public ActionResult DeleteImage(Guid id)
		{
			var removeImage = db.ImageProducts.SingleOrDefault(t => t.Id == id);
			db.ImageProducts.Remove(removeImage);
			db.SaveChanges();
			TempData["success"] = "Delete Successfully!";
			return RedirectToAction("Details", new { id = removeImage.ProductId });
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