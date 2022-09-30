using eCommerceProject.Areas.Admin.Services;
using eCommerceProject.Enums;
using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationDbContext _context;
		private ApplicationDbContext db = new ApplicationDbContext();
		SharedServices service = new SharedServices();
		//public HomeController()
		//{
		//	_user = new ApplicationUser();

		//	_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		//}
		public ActionResult Index(Customer user)
		{

			return View();
		}
		//-------------------------------------------------------------
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}
		//-------------------------------------------------------------
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		//-------------------------------------------------------------
		public ActionResult ProductDetail()
		{
			return View();
		}
		//-------------------------------------------------------------
		public ActionResult BannerSlider()
		{
			var banner = db.BannerSliders.ToList();
			return View(banner);
		}
		//-------------------------------------------------------------
		public ActionResult _footer()
		{


			return View();
		}
		//-------------------------------------------------------------
		public ActionResult PopUpNoti()
		{


			return View();
		}


		public ActionResult CategoryNavbar()
		{
			var categories = db.Categories.ToList();

			return View(categories);
		}
		//-------------------------------------------------------------
		public ActionResult Category()
		{
			var banner = db.Categories.ToList();

			return View(banner);
		}
		//-------------------------------------------------------------
		public ActionResult BestSeller()
		{
			int recordCount = 4;
			var bestSeller = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Where(x => x.BestSellers).Take(recordCount).ToList();
			ViewBag.product = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Where(x => x.BestSellers).Take(recordCount).ToList();
			return View(bestSeller);
		}
		//-------------------------------------------------------------
		public ActionResult NewestProduct()
		{
			int recordCount = 4;
			var bestSeller = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Take(recordCount).ToList();
			ViewBag.product = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Take(recordCount).ToList();
			return View(bestSeller);
		}
		//-------------------------------------------------------------
		public ActionResult DetailsProduct(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Home");
			}
			var product = new ViewModel.ProductVM();
			product.Product = db.Products.
							Include(i => i.Categories).
							Include(i => i.Sizes).
						SingleOrDefault(t => t.Id == id);
			//comment product
			ViewBag.ProductId = id.Value;
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
				return RedirectToAction("Error", "Home");
			}
			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddCommentRating(FormCollection form)
		{
			var comment = form["Comment"].ToString();
			var articleId = int.Parse(form["ProductId"]);
			var rating = int.Parse(form["Rating"]);
			var userId = User.Identity.GetUserId();
			StarRatingAndComment artComment = new StarRatingAndComment()
			{
				ProductId = articleId,
				Comments = comment,
				Rating = rating,
				ThisDateTime = DateTime.Now,
				UserId = userId,
				isPublic = false
			};

			db.StarRatingAndComments.Add(artComment);
			db.SaveChanges();

			return RedirectToAction("DetailsProduct", "Home", new { id = articleId });
		}
		//delete comment
		public ActionResult DeleteCommentandRating(int? id)
		{
			var product = new ViewModel.ProductVM();
			product.Product = db.Products.ToList().Find(u => u.Id == id);
			if (ModelState.IsValid)
			{
				string currentUserId = User.Identity.GetUserId();
				var removeCmt = db.StarRatingAndComments
					.Where(x => x.UserId == currentUserId)
					.SingleOrDefault(t => t.Id == id);
				db.StarRatingAndComments.Remove(removeCmt);
				db.SaveChanges();
				return RedirectToAction("DetailsProduct", "", new { id = removeCmt.ProductId });
			}

			return View(id);
		}

		//-------------------------------------------------------------
		//product categories
		public ActionResult ProductCategories(int? id, string Sorting_Order, string search, int? filter, string Search_Data, string Filter_Value, int? Page_No)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Home");
			}

			//viewbag
			ViewBag.CurrentSortOrder = Sorting_Order;
			ViewBag.SortingNameAZ = String.IsNullOrEmpty(Sorting_Order) ? "ProductNameAZ" : "";
			ViewBag.SortingNameZA = String.IsNullOrEmpty(Sorting_Order) ? "ProductNameZA" : "";
			ViewBag.SortingPriceLowtoHigh = String.IsNullOrEmpty(Sorting_Order) ? "PriceLowtoHigh" : "";
			ViewBag.SortingPriceHightoLow = String.IsNullOrEmpty(Sorting_Order) ? "PriceHightoLow" : "";
			ViewBag.SortingNewsestProduct = String.IsNullOrEmpty(Sorting_Order) ? "NewsestProduct" : "";
			ViewBag.SortingBestSellerProduct = String.IsNullOrEmpty(Sorting_Order) ? "BestSellerProduct" : "";
			ViewBag.SortingDate = Sorting_Order == "CreatedDate" ? "ProductName" : "Date";
			ViewData["DanhMuc"] = new SelectList(db.Categories, "Id", "CategoryName", "ImagePath");
			//
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

			var products = from stu in db.Products.Include(t => t.Categories).AsNoTracking().OrderByDescending(t => t.CreatedDate).Where(i => i.CategoriesID == id).ToList() select stu;


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
				case "ProductNameZA":
					products = products.OrderBy(stu => stu.ProductName);
					break;
				case "ProductNameAZ":
					products = products.OrderByDescending(stu => stu.ProductName);
					break;
				case "NewsestProduct":
					products = products.OrderByDescending(stu => stu.CreatedDate);
					break;
				case "BestSellerProduct":
					products = products.Where(stu => stu.BestSellers == true).OrderBy(stu => stu.CreatedDate);
					break;
				case "PriceLowtoHigh":
					products = products.OrderBy(stu => stu.Price);
					break;
				case "PriceHightoLow":
					products = products.OrderByDescending(stu => stu.Price);
					break;
				default:
					products = products.OrderBy(stu => stu.CreatedDate);
					break;
			}

			int Size_Of_Page = 16;
			int No_Of_Page = (Page_No ?? 1);
			if (products == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
		}
		//-------------------------------------------------------------

		//-------------------------------------------------------------
		public ActionResult AllProduct(string Sorting_Order, string search, int? filter, string Search_Data, string Filter_Value, int? Page_No)
		{
			ViewBag.CurrentSortOrder = Sorting_Order;
			ViewBag.SortingNameAZ = String.IsNullOrEmpty(Sorting_Order) ? "ProductNameAZ" : "";
			ViewBag.SortingNameZA = String.IsNullOrEmpty(Sorting_Order) ? "ProductNameZA" : "";
			ViewBag.SortingPriceLowtoHigh = String.IsNullOrEmpty(Sorting_Order) ? "PriceLowtoHigh" : "";
			ViewBag.SortingPriceHightoLow = String.IsNullOrEmpty(Sorting_Order) ? "PriceHightoLow" : "";
			ViewBag.SortingNewsestProduct = String.IsNullOrEmpty(Sorting_Order) ? "NewsestProduct" : "";
			ViewBag.SortingBestSellerProduct = String.IsNullOrEmpty(Sorting_Order) ? "BestSellerProduct" : "";
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
			var products = from stu in db.Products.Include(t => t.Categories).AsNoTracking().OrderByDescending(t => t.CreatedDate).ToList() select stu;
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
				case "ProductNameZA":
					products = products.OrderBy(stu => stu.ProductName);
					break;
				case "ProductNameAZ":
					products = products.OrderByDescending(stu => stu.ProductName);
					break;
				case "NewsestProduct":
					products = products.OrderByDescending(stu => stu.CreatedDate);
					break;
				case "BestSellerProduct":
					products = products.Where(stu => stu.BestSellers == true).OrderBy(stu => stu.CreatedDate);
					break;
				case "PriceLowtoHigh":
					products = products.OrderBy(stu => stu.Price);
					break;
				case "PriceHightoLow":
					products = products.OrderByDescending(stu => stu.Price);
					break;
				default:
					products = products.OrderBy(stu => stu.CreatedDate);
					break;
			}

			int Size_Of_Page = 16;
			int No_Of_Page = (Page_No ?? 1);
			return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
		}

		//-------------------------------------------------------------
		public ActionResult ListBlog()
		{
			int recordCount = 3;
			var blog = db.BlogPosts.Where(t => t.isHotNews == true && t.isPublic == true).OrderByDescending(t => t.CreateDate).Take(recordCount).ToList();

			return View(blog);
		}
		//-------------------------------------------------------------
		//Details BlogCategories
		public ActionResult DetailsBlogCategories(int? id, string search, int? filter, int? page)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Home");
			}
			var blog = from s in db.BlogPosts.Where(t => t.isHotNews == true && t.isPublic == true && t.BlogCategoriesID == id).Include(t => t.BlogCategories).Include(t => t.User).OrderByDescending(t => t.CreateDate).ToList() select s;
			if (!String.IsNullOrWhiteSpace(search))
			{
				blog = blog.Where(s => s.Title.Contains(search)
															 || s.ShortContents.Contains(search)).ToList();
			}

			if (blog == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(blog.ToPagedList(page ?? 1, 5));
		}
		//-------------------------------------------------------------
		public ActionResult Blog(int? page, string search, int? filter)
		{

			var blogPost = from s in db.BlogPosts.Include(t => t.User).Include(x => x.BlogCategories).AsNoTracking().OrderByDescending(t => t.CreateDate).ToList()
										 select s;

			if (!String.IsNullOrWhiteSpace(search))
			{
				blogPost = blogPost.Where(s => s.Title.Contains(search)
															 || s.ShortContents.Contains(search)).ToList();
			}



			return View(blogPost.ToPagedList(page ?? 1, 5));
		}
		//-------------------------------------------------------------
		public ActionResult DetailsBlog(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Home");
			}
			//update view
			var course = new ViewModel.BlogVM();
			BlogPost update = db.BlogPosts.ToList().Find(u => u.Id == id);
			if (update == null)
			{
				var a = User.Identity.GetUserId();
				course.EntityID = (int)EntityEnums.Post;
				course.BlogPost = db.BlogPosts.
								Include(i => i.BlogCategories).
								Include(i => i.User).
							SingleOrDefault(t => t.Id == id);
				course.Comments = service.GetComments((int)EntityEnums.Post, course.BlogPost.Id);
				course.UserName = User.Identity.GetUserId();
				return View(course);
			}
			else
			{
				update.BlogPostView += 1;
				db.SaveChanges();

				// count comment
				var cmtCount = db.Comments.Where(t => t.RecordID == update.Id).ToList();
				ViewBag.CmtCount = cmtCount.Count();
				//get details
				var a = User.Identity.GetUserId();
				course.EntityID = (int)EntityEnums.Post;
				course.BlogPost = db.BlogPosts.
								Include(i => i.BlogCategories).
								Include(i => i.User).
							SingleOrDefault(t => t.Id == id);
				course.Comments = service.GetComments((int)EntityEnums.Post, course.BlogPost.Id);
				course.UserName = User.Identity.GetUserId();
				//get details


				return View(course);
			}

			if (course == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(course);
		}
		//-------------------------------------------------------------
		//comment
		[HttpPost]
		public JsonResult LeaveComment(CommentViewModels model, string Text)
		{
			JsonResult result = new JsonResult();


			try
			{
				var comment = new Comments();
				comment.Id = model.CommentID;
				comment.Text = Text;
				comment.RecordID = model.RecordID;
				comment.EntityID = model.EntityID;
				comment.UserID = User.Identity.GetUserId();
				comment.TimeStamp = DateTime.Now;

				var res = service.LeaveComment(comment);
				result.Data = new { Success = res };
			}
			catch (Exception ex)
			{
				result.Data = new { Success = false, Message = ex.Message };
			}

			return result;
		}

		//-------------------------------------------------------------
		public ActionResult DeleteComment(int id)
		{
			BlogPost update = db.BlogPosts.ToList().Find(u => u.Id == id);
			if (ModelState.IsValid)
			{
				string currentUserId = User.Identity.GetUserId();
				var removeCmt = db.Comments
					.Where(x => x.UserID == currentUserId)
					.SingleOrDefault(t => t.Id == id);
				db.Comments.Remove(removeCmt);
				db.SaveChanges();

			}
			return View(update);
		}
		//-------------------------------------------------------------
		//BLOG FEature
		public ActionResult ProductFeature()
		{
			int recordCount = 3;
			var blog = db.Products.Where(t => t.BestSellers == true && t.Active == true).Include(t => t.ImageProducts).OrderBy(t => t.CreatedDate).Take(recordCount).ToList();

			return View(blog);
		}
		//-------------------------------------------------------------
		public ActionResult CategoriesFeature()
		{
			int recordCount = 3;
			var blogCategory = db.BlogCategories.Take(recordCount).ToList();

			return View(blogCategory);
		}
		//-------------------------------------------------------------

		// RELATED PRODUCT
		public ActionResult RelatedProduct(int? id)
		{
			//int recordCount = 4;
			var idCategories = db.Products.Where(t => t.Id == id).SingleOrDefault();
			var relateProduct = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Where(t => t.CategoriesID == idCategories.CategoriesID).ToList();
			ViewBag.product = db.Products.OrderBy(x => x.CreatedDate).Include(t => t.ImageProducts).Where(t => t.CategoriesID == idCategories.CategoriesID).ToList();
			return View(relateProduct);
		}
		//-------------------------------------------------------------
		// error page
		public ActionResult Error()
		{
			ViewBag.Error = "Something didn't find here :(";
			return View();
		}
		//-------------------------------------------------------------
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