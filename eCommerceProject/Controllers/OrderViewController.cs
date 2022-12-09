using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace eCommerceProject.Controllers
{
	public class OrderViewController : Controller
	{

		private ApplicationDbContext db = new ApplicationDbContext();
		private ApplicationDbContext _context;
		public ApplicationDbContext context
					 => _context ?? (_context = ApplicationDbContext.Create());


		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Home");
			}
			try
			{
				var taikhoanID = User.Identity.GetUserId();
				if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");
				var khachhang = db.Customers.AsNoTracking().SingleOrDefault(x => x.Id == taikhoanID);
				if (khachhang == null) return RedirectToAction("Error", "Home");

				var donhang = await db.Orders
						.Include(x => x.TransactStatus)
						.FirstOrDefaultAsync(m => m.OrderId == id && taikhoanID == m.CustomerId);
				if (donhang == null) return RedirectToAction("Error", "Home");

				var chitietdonhang = db.OrderDetails
						.Include(x => x.Product)
						.Include(x => x.Size)
						.AsNoTracking()
						.Where(x => x.OrderId == id)
						.OrderBy(x => x.OrderDetailId)
						.ToList();
				OrderViewModel donHang = new OrderViewModel();
				donHang.Orders = donhang;
				donHang.OrderDetails = chitietdonhang;
				return PartialView("Details", donHang);

			}
			catch
			{
				return RedirectToAction("Error", "Home");
			}
		}
		public ActionResult CancelOrder(int id)
		{
			Order cancleOrder = db.Orders.ToList().Find(t => t.OrderId == id);
			cancleOrder.TransactStatusId = 5;
			db.SaveChanges();
			TempData["success"] = "Cancel Order Success!";
			return View(cancleOrder);
		}
	}
}