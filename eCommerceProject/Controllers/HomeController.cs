using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		public HomeController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		public ActionResult BannerSlider()
		{
			var banner = _context.BannerSliders.ToList();
			return View(banner);
		}
		public ActionResult ProductDetail()
		{
			return View();
		}

		public ActionResult _footer()
		{


			return View();
		}
	}
}