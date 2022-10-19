using eCommerceProject.Models;
using eCommerceProject.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;


namespace eCommerceProject.Controllers
{

	public class CheckoutController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Checkout
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
		[Route("checkout.html", Name = "Checkout")]
		public ActionResult Index(string returnUrl = null)
		{
			//Lay gio hang ra de xu ly
			var gh = Session["GioHang"] as List<Cart>;
			//string sessionID = HttpContext.Session.SessionID;

			var taikhoanID = User.Identity.GetUserId();

			MuaHangVM model = new MuaHangVM();
			if (taikhoanID != null)
			{


				var khachhang = db.Customers.AsNoTracking().SingleOrDefault(x => x.Id == taikhoanID);
				model.CustomerId = khachhang.Id;
				model.FullName = khachhang.FullName;
				model.Email = khachhang.Email;
				model.PhoneNumber = khachhang.PhoneNumber;
				model.Address = khachhang.Address;
			}
			ViewData["lsTinhThanh"] = new SelectList(db.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
			ViewBag.GioHang = gh;
			return View(model);
		}

		[HttpPost]
		[Route("checkout.html", Name = "Checkout")]
		public ActionResult Index(MuaHangVM muaHang)
		{
			//Lay ra gio hang de xu ly
			var gh = Session["GioHang"] as List<Cart>;
			var taikhoanID = User.Identity.GetUserId();
			MuaHangVM model = new MuaHangVM();
			if (taikhoanID != null)
			{
				var khachhang = db.Customers.AsNoTracking().SingleOrDefault(x => x.Id == taikhoanID);
				model.CustomerId = khachhang.Id;
				model.FullName = khachhang.FullName;

				model.Email = khachhang.Email;
				model.PhoneNumber = khachhang.PhoneNumber;
				model.Address = khachhang.Address;

				khachhang.Address = muaHang.Address;
				db.Entry(khachhang).State = EntityState.Modified;
				db.SaveChanges();
			}
			//try
			//{
			if (ModelState.IsValid)
			{
				//Khoi tao don hang
				Order donhang = new Order();
				donhang.CustomerId = model.CustomerId;
				donhang.Address = model.Address;


				donhang.OrderDate = DateTime.Now;
				donhang.TransactStatusId = 1;//Don hang moi
				donhang.Deleted = false;
				donhang.Paid = false;
				donhang.Note = model.Note;
				//donhang.Note = Utilities.StripHTML(model.Note);
				donhang.TotalMoney = Convert.ToInt32(gh.Sum(x => x.TotalMoney));
				db.Orders.Add(donhang);
				db.SaveChanges();
				//tao danh sach don hang
				string body = string.Empty;
				using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OrderTemplate.html")))
				{
					body = reader.ReadToEnd();
				}

				foreach (var item in gh)
				{
					OrderDetail orderDetail = new OrderDetail();
					orderDetail.OrderId = donhang.OrderId;
					orderDetail.ProductId = item.Product.Id;
					orderDetail.Amount = item.amount;
					orderDetail.TotalMoney = donhang.TotalMoney;
					orderDetail.Price = item.Product.Price;
					orderDetail.CreateDate = DateTime.Now;
					db.OrderDetails.Add(orderDetail);

					//valueProvider for email
					body = body.Replace("{ProductName}", item.Product.ProductName);
					body = body.Replace("{ProductPrice}", item.Product.PriceSale.ToString("#,##0"));
					body = body.Replace("{Quantity}", Convert.ToString(item.amount));

				}
				db.SaveChanges();
				//mail

				var viewId = "/OrderView/Details/" + donhang.OrderId;

				body = body.Replace("{UserName}", donhang.Customer.FullName);
				body = body.Replace("{Address}", donhang.Customer.Address);
				body = body.Replace("{PhoneNumber}", donhang.Customer.PhoneNumber);
				body = body.Replace("{OrderId}", Convert.ToString(donhang.OrderId));
				body = body.Replace("{Total}", donhang.TotalMoney.ToString("#,##0"));
				body = body.Replace("{OrderDate}", Convert.ToString(donhang.OrderDate));
				body = body.Replace("{View}", viewId);
				using (MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
					mailMessage.To.Add(new MailAddress(donhang.Customer.Email));
					mailMessage.Subject = "Thanks for your order - COCO Store";
					mailMessage.Body = body;
					mailMessage.IsBodyHtml = true;
					//mailMessage.To.Add(new MailAddress(email.Recipient));
					SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
					System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
					smtpClient.Credentials = credentials;
					smtpClient.EnableSsl = true;

					smtpClient.Send(mailMessage);
				}
				//clear gio hang
				HttpContext.Session.Remove("GioHang");
				//Xuat thong bao
				TempData["success"] = "Order Success!";
				//cap nhat thong tin khach hang
				return RedirectToAction("Success");


			}
			//}
			//catch
			//{
			//	ViewData["lsTinhThanh"] = new SelectList(db.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
			//	ViewBag.GioHang = gh;
			//	return View(model);
			//}
			//ViewData["lsTinhThanh"] = new SelectList(db.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
			//ViewBag.GioHang = gh;
			var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
	.SelectMany(E => E.Errors)
	.Select(E => E.ErrorMessage)
	.ToList();
			return View(model);
		}
		[Route("dat-hang-thanh-cong.html", Name = "Success")]
		public ActionResult Success()
		{
			try
			{
				var taikhoanID = User.Identity.GetUserId();
				if (string.IsNullOrEmpty(taikhoanID))
				{
					return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
				}
				var khachhang = db.Customers.AsNoTracking().SingleOrDefault(x => x.Id == taikhoanID);
				var donhang = db.Orders
						.Where(x => x.CustomerId == taikhoanID)
						.OrderByDescending(x => x.OrderDate)
						.FirstOrDefault();
				MuaHangSuccessVM successVM = new MuaHangSuccessVM();
				successVM.FullName = khachhang.FullName;
				successVM.DonHangID = donhang.OrderId;
				successVM.Phone = khachhang.PhoneNumber;
				successVM.Address = khachhang.Address;

				return View(successVM);
			}
			catch
			{
				return View();
			}
		}
	}

}