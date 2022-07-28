using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
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
			return View();
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
		[Bind(Include = "Id,Title,Description,ImageFile")] BannerSlider banner)
		{
			if (ModelState.IsValid)
			{
				string fileName = Path.GetFileNameWithoutExtension(banner.ImageFile.FileName);
				string exe = Path.GetExtension(banner.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				banner.ImagePath = "~/Content/ImageProduct/Banner/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Banner/"), fileName);
				banner.ImageFile.SaveAs(fileName);

				var post = _context.BannerSliders.FirstOrDefault(t => t.Id == banner.Id);
				post.Title = banner.Title;
				post.Description = banner.Description;
				post.ImagePath = banner.ImagePath;


				_context.SaveChanges();
				TempData["success"] = "Update Image Successfully";
				return RedirectToAction("BannerManage", "Admin");
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
