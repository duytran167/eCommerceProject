using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace eCommerceProject.Controllers
{
	public class CartController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: ShoppingCart

		public List<Cart> GioHang
		{
			get
			{
				var gh = Session["GioHang"] as List<Cart>;
				if (gh == default(List<Cart>))
				{
					gh = new List<Cart>();
				}
				return gh;
			}
		}
		public ActionResult AddToCart(int SanPhamID, int? amount, string size)
		{
			if (Session["giohang"] == null) // Nếu giỏ hàng chưa được khởi tạo
			{
				Session["giohang"] = new List<Cart>(); // Khởi tạo Session["giohang"] là 1 List<CartItem>
			}

			List<Cart> giohang = Session["giohang"] as List<Cart>; // Gán qua biến giohang dễ code


			// Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa

			if (giohang.FirstOrDefault(m => m.Product.Id == SanPhamID) == null) // ko co sp nay trong gio hang
			{
				Product sp = db.Products
					.Find(SanPhamID); // tim sp theo sanPhamID
														//Size size = db.Sizes.Find(SizeID);
				Cart newItem = new Cart()
				{
					ProductId = SanPhamID,
					Product = sp,
					amount = amount.HasValue ? amount.Value : 1,
					//Size = size


				}; // Tạo ra 1 CartItem mới

				giohang.Add(newItem); // Thêm CartItem vào giỏ
				TempData["success"] = "Add Cart Success!";
			}
			else
			{
				// Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
				Cart cardItem = giohang.FirstOrDefault(m => m.Product.Id == SanPhamID);

				cardItem.amount = cardItem.amount + amount.Value;
				TempData["success"] = "Add Cart Success!";

			}

			// Action này sẽ chuyển hướng về trang chi tiết sp khi khách hàng đặt vào giỏ thành công. Bạn có thể chuyển về chính trang khách hàng vừa đứng bằng lệnh return Redirect(Request.UrlReferrer.ToString()); nếu muốn.
			return Json(RedirectToAction("DetailsProduct", "Home", new { id = SanPhamID }));


		}


		public JsonResult Delete(long id)
		{
			var sessionCart = (List<Cart>)Session["giohang"];
			sessionCart.RemoveAll(x => x.Product.Id == id);
			Session["giohang"] = sessionCart;
			return Json(new
			{
				status = true
			});
		}
		public RedirectToRouteResult UpdateCart(int SanPhamID, int soluongmoi)
		{
			// tìm cart item muon sua
			List<Cart> giohang = Session["giohang"] as List<Cart>;
			Cart itemSua = giohang.FirstOrDefault(m => m.Product.Id == SanPhamID);
			if (itemSua != null)
			{
				itemSua.amount = soluongmoi;
			}
			return RedirectToAction("Index");

		}
		public JsonResult Update(string cartModel)
		{
			var jsonCart = new JavaScriptSerializer().Deserialize<List<Cart>>(cartModel);
			var sessionCart = (List<Cart>)Session["giohang"];

			foreach (var item in sessionCart)
			{
				var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
				if (jsonItem != null)
				{
					item.amount = jsonItem.amount;
				}
			}
			Session["giohang"] = sessionCart;
			return Json(new
			{
				status = true
			});
		}
		public ActionResult CountItem()
		{
			List<Cart?> giohang = Session["GioHang"] as List<Cart?>;

			return View(giohang);

		}

		public ActionResult Index()
		{
			List<Cart> giohang = Session["GioHang"] as List<Cart>;
			return View(giohang);

		}

	}

}