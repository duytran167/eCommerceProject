using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace eCommerceProject.Controllers
{
	public class ShoppingCartController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: ShoppingCart

		public List<CartItem> GioHang
		{
			get
			{
				var gh = Session["GioHang"] as List<CartItem>;
				if (gh == default(List<CartItem>))
				{
					gh = new List<CartItem>();
				}
				return gh;
			}
		}
		[HttpPost]
		[Route("api/cart/add")]
		public ActionResult AddToCart(int productID, int? amount)
		{
			List<CartItem> cart = GioHang;

			try
			{
				//Them san pham vao gio hang
				CartItem item = cart.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null) // da co => cap nhat so luong
				{
					item.amount = item.amount + amount.Value;
					//luu lai session
					Session["GioHang"] = new List<CartItem>(cart);
				}
				else
				{
					Product hh = db.Products.SingleOrDefault(p => p.Id == productID);
					item = new CartItem
					{
						amount = amount.HasValue ? amount.Value : 1,
						Product = hh
					};
					cart.Add(item);//Them vao gio
				}

				//Luu lai Session
				Session["GioHang"] = new List<CartItem>(cart);

				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[HttpPost]
		[Route("api/cart/update")]
		public ActionResult UpdateCart(int productID, int? amount)
		{
			//Lay gio hang ra de xu ly
			var cart = Session["Giohang"] as List<CartItem>;
			try
			{
				if (cart != null)
				{
					CartItem item = cart.SingleOrDefault(p => p.Product.Id == productID);
					if (item != null && amount.HasValue) // da co -> cap nhat so luong
					{
						item.amount = amount.Value;
					}
					//Luu lai session
					Session["GioHang"] = new List<CartItem>(cart);
				}
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}

		[HttpPost]
		[Route("api/cart/remove")]
		public ActionResult Remove(int productID)
		{

			try
			{
				List<CartItem> gioHang = GioHang;
				CartItem item = gioHang.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null)
				{
					gioHang.Remove(item);
				}
				//luu lai session
				Session["GioHang"] = new List<CartItem>(gioHang);

				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[Route("cart.html", Name = "Cart")]
		public ActionResult Index()
		{
			return View(GioHang);
		}

	}

}