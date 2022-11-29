using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Seller")]

	public class AdminController : Controller
	{
		// GET: Admin
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		public AdminController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		public ActionResult Index()
		{
			var now = DateTime.Now;
			var first = new DateTime(now.Year, now.Month, 1);
			var last = first.AddMonths(1).AddDays(-1);
			var orderbyMonth = (from u in db.OrderDetails
													where u.CreateDate < last
													where u.CreateDate > first
													select u).ToList();
			var totalMoneyByMonth = (from u in db.OrderDetails

															 where u.CreateDate < last
															 where u.CreateDate > first
															 select u.TotalMoney).ToList();
			var totalUserByMonth = (from u in db.Customers

															where u.CreatedDate < last
															where u.CreatedDate > first
															select u).ToList();
			var totalItemSellByMonth = (from u in db.OrderDetails

																	where u.CreateDate < last
																	where u.CreateDate > first
																	select u.Amount).ToList();
			ViewBag.CountOrderByMonth = orderbyMonth.Count();
			ViewBag.TotalMoneyByMonth = totalMoneyByMonth.Sum();
			ViewBag.TotalUserByMonth = totalUserByMonth.Count();
			ViewBag.TotalItemSellByMonth = totalItemSellByMonth.Count();
			return View();
		}
		public ActionResult GetDataByCategories()
		{

			int top = db.Products.Include(t => t.Categories).Where(x => x.Categories.CategoryName == "TOP").Count();
			int outerwear = db.Products.Include(t => t.Categories).Where(x => x.Categories.CategoryName == "OUTERWEAR").Count();
			int bottoms = db.Products.Include(t => t.Categories).Where(x => x.Categories.CategoryName == "BOTTOMS").Count();
			int accessories = db.Products.Include(t => t.Categories).Where(x => x.Categories.CategoryName == "ACCESSORIES").Count();
			Cate obj = new Cate();
			obj.TOP = top;
			obj.OUTERWEAR = outerwear;
			obj.BOTTOMS = bottoms;
			obj.ACCESSORIES = accessories;
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetDataTotalMoney()
		{


			var query = db.OrderDetails.Include(t => t.Product)
						 .GroupBy(p => p.Product.ProductName)
						 .Select(g => new { name = g.Key, count = g.Sum(w => w.TotalMoney) }).ToList();
			return Json(query, JsonRequestBehavior.AllowGet);
		}
		public class Cate
		{
			public int TOP { get; set; }
			public int OUTERWEAR { get; set; }
			public int BOTTOMS { get; set; }
			public int ACCESSORIES { get; set; }
		}
		public ActionResult GetDataBestSeller()
		{
			var query = db.OrderDetails.Include(t => t.Product)
						 .GroupBy(p => p.Product.ProductName)
						 .Select(g => new { name = g.Key, count = g.Sum(w => w.Amount) }).ToList();
			return Json(query, JsonRequestBehavior.AllowGet);
		}
		// GET: Admin/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}




		//-------------------------------- Banner-------------------------------------------------------
		public ActionResult BannerManage()
		{

			var banner = _context.BannerSliders.OrderByDescending(m => m.ArticleDate).ToList();

			return View(banner);
		}
		[HttpGet]
		public ActionResult CreateBanner()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateBanner([Bind(Include = "Id,Title,Description,ArticleDate,ImageFile")] BannerSlider banner)
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

				var newBanner = new BannerSlider()
				{
					Title = banner.Title,
					Description = banner.Description,
					ArticleDate = banner.ArticleDate,
					ImagePath = banner.ImagePath,
				};
				db.BannerSliders.Add(newBanner);
				db.SaveChanges();
				_context.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("BannerManage");
			}

			return View(banner);

		}
		public ActionResult DeleteBanner(int id)
		{
			var removeBanner = _context.BannerSliders.SingleOrDefault(t => t.Id == id);
			_context.BannerSliders.Remove(removeBanner);
			_context.SaveChanges();
			TempData["error"] = "Delete Successfully!";
			return RedirectToAction("BannerManage");
		}
		[HttpGet]
		public ActionResult UpdateBanner(int id)
		{
			var banner = _context.BannerSliders
							 .SingleOrDefault(t => t.Id == id);
			var updateBanner = new BannerSlider()
			{
				Title = banner.Title,
				Description = banner.Description,
				ImagePath = banner.ImagePath,



			};

			return View(updateBanner);
		}
		[HttpPost]
		public async Task<ActionResult> UpdateBanner(
		[Bind(Include = "Id,Title,Description,ImageFile")] BannerSlider banner, HttpPostedFileBase fileImage)
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

					var post = _context.BannerSliders.FirstOrDefault(t => t.Id == banner.Id);
					post.Title = banner.Title;
					post.Description = banner.Description;
					post.ImagePath = banner.ImagePath;


					_context.SaveChanges();
					TempData["success"] = "Update Image Successfully";
					return RedirectToAction("BannerManage", "Admin");
				}
				else
				{
					var post = _context.BannerSliders.FirstOrDefault(t => t.Id == banner.Id);
					post.Title = banner.Title;
					post.Description = banner.Description;



					_context.SaveChanges();
					TempData["success"] = "Update Image Successfully";
					return RedirectToAction("BannerManage", "Admin");
				}
			}
			return View(banner);
		}

		// GET: Admin/Create
		public ActionResult Error()
		{


			return View();
		}
	}
}
