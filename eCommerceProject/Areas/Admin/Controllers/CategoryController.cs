using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Seller")]
	public class CategoryController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		public CategoryController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		// GET: Category
		public ActionResult Index()
		{

			var banner = _context.Categories.ToList();

			return View(banner);
		}
		[HttpGet]
		public ActionResult CreateCategory()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateCategory([Bind(Include = "Id,CategoryName,ImageFile")] Category category)
		{

			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
				string exe = Path.GetExtension(category.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				category.ImagePath = "~/Content/ImageProduct/Category/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Category/"), fileName);
				category.ImageFile.SaveAs(fileName);

				var newCategory = new Category()
				{
					CategoryName = category.CategoryName,
					ImagePath = category.ImagePath,
				};
				db.Categories.Add(newCategory);
				db.SaveChanges();
				_context.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Index");
			}

			return View(category);

		}
		public ActionResult DeleteCategory(int id)
		{
			var removeCategory = _context.Categories.SingleOrDefault(t => t.Id == id);
			_context.Categories.Remove(removeCategory);
			_context.SaveChanges();
			TempData["error"] = "Delete Successfully!";
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult UpdateCategory(int id)
		{
			var category = _context.Categories
							 .SingleOrDefault(t => t.Id == id);
			var updateBanner = new Category()
			{
				CategoryName = category.CategoryName,
				ImagePath = category.ImagePath,

			};

			return View(updateBanner);
		}
		[HttpPost]
		public async Task<ActionResult> UpdateCategory(
		[Bind(Include = "Id,CategoryName,ImageFile")] Category category, HttpPostedFileBase fileImage)
		{
			if (ModelState.IsValid)
			{
				if (fileImage != null && fileImage.ContentLength > 0)
				{
					string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
					string exe = Path.GetExtension(category.ImageFile.FileName);
					fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
					category.ImagePath = "~/Content/ImageProduct/Category/" + fileName;
					fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/Category/"), fileName);
					category.ImageFile.SaveAs(fileName);

					var post = _context.Categories.FirstOrDefault(t => t.Id == category.Id);
					post.CategoryName = category.CategoryName;
					post.ImagePath = category.ImagePath;


					_context.SaveChanges();
					TempData["success"] = "Update Success!";
					return RedirectToAction("Index", "Category");
				}
				else
				{
					var post = _context.Categories.FirstOrDefault(t => t.Id == category.Id);
					post.CategoryName = category.CategoryName;



					_context.SaveChanges();
					TempData["success"] = "Update Success!";
					return RedirectToAction("Index", "Category");
				}
			}
			return View(category);
		}
	}
}