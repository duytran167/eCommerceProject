using System.Web.Mvc;

namespace eCommerceProject.Controllers
{
	public class HomeController : Controller
	{
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