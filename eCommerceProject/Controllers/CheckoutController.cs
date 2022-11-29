using eCommerceProject.Models;
using eCommerceProject.Others;
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
using System.Text;
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
			ViewBag.Size = db.Sizes.ToList();

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
				var body = new StringBuilder();
				var viewId = "/OrderView/Details/" + donhang.OrderId;





				foreach (var item in gh)
				{
					OrderDetail orderDetail = new OrderDetail();
					orderDetail.OrderId = donhang.OrderId;
					orderDetail.ProductId = item.Product.Id;
					orderDetail.Amount = item.amount;
					orderDetail.SizeId = item.SizeId;
					var sizeName = db.Sizes.Where(t => t.Id == item.SizeId);
					//value to get from string text fopr email
					using (var reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OrderTemplate.html")))
					{
						body.Append(reader.ReadToEnd());
						foreach (var i in sizeName)
						{
							var sizename = i.SizeName;

							body.Replace("{Size}", Convert.ToString(sizename));

						}


						orderDetail.TotalMoney = item.Product.PriceSale * item.amount;
						orderDetail.Price = item.Product.PriceSale;
						orderDetail.CreateDate = DateTime.Now;

						//tru di stock có sẵn của product
						Size size = db.Sizes.ToList().Find(t => t.ProductId == item.ProductId);
						size.Stock -= item.amount;


						//get size name

						db.OrderDetails.Add(orderDetail);

						//valueProvider for email


						body.Replace("{UserName}", donhang.Customer.FullName);
						body.Replace("{Address}", donhang.Customer.Address);
						body.Replace("{PhoneNumber}", donhang.Customer.PhoneNumber);
						body.Replace("{OrderId}", Convert.ToString(donhang.OrderId));
						body.Replace("{Total}", donhang.TotalMoney.ToString("#,##0"));
						body.Replace("{OrderDate}", Convert.ToString(donhang.OrderDate));
						body.Replace("{View}", viewId);

						body.Replace("{ProductName}", item.Product.ProductName);
						body.Replace("{ProductPrice}", (item.Product.PriceSale * item.amount).ToString("#,##0"));
						body.Replace("{ProductForEachPrice}", item.Product.PriceSale.ToString("#,##0"));
						body.Replace("{Quantity}", Convert.ToString(item.amount));

						body.Append("<br />");
					}

				}

				db.SaveChanges();
				//mail
				//declare body fgor email






				using (MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
					mailMessage.To.Add(new MailAddress(donhang.Customer.Email));
					mailMessage.Subject = "Thanks for your order - COCO Store";
					mailMessage.Body = body.ToString();
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
		//thanh toan vnpay
		[HttpPost]
		public ActionResult Payment(MuaHangVM muaHang)
		{
			//get value form vn pay



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
				model.PaymentMethod = muaHang.PaymentMethod;
				khachhang.Address = muaHang.Address;
				db.Entry(khachhang).State = EntityState.Modified;
				db.SaveChanges();
			}
			//try
			//{
			var body = new StringBuilder();
			if (muaHang.PaymentMethod == "VNPAY")
			{


				if (ModelState.IsValid)
				{
					//Khoi tao don hang
					Order donhang = new Order();
					donhang.CustomerId = model.CustomerId;
					donhang.Address = model.Address;
					donhang.PaymentMethod = model.PaymentMethod;
					donhang.Noti = false;
					donhang.OrderDate = DateTime.Now;
					donhang.TransactStatusId = 1;//Don hang moi
					donhang.Deleted = false;
					donhang.Paid = true;
					donhang.Note = muaHang.Note;
					//donhang.Note = Utilities.StripHTML(model.Note);
					donhang.TotalMoney = Convert.ToInt32(gh.Sum(x => x.TotalMoney));

					//passs order to money tro vnpay
					string url = ConfigurationManager.AppSettings["Url"];
					string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
					string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
					string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

					PayLib pay = new PayLib();

					pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
					pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
					pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
					pay.AddRequestData("vnp_Amount", Convert.ToString(donhang.TotalMoney * 100)); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
					pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
					pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
					pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
					pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
					pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
					pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
					pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
					pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
					pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

					db.Orders.Add(donhang);
					db.SaveChanges();


					//tao danh sach don hang

					//var viewId = "/OrderView/Details/" + donhang.OrderId;

					string paymentUrl = pay.CreateRequestUrl(url, hashSecret);



					foreach (var item in gh)
					{
						OrderDetail orderDetail = new OrderDetail();
						orderDetail.OrderId = donhang.OrderId;
						orderDetail.ProductId = item.Product.Id;
						orderDetail.Amount = item.amount;
						orderDetail.SizeId = item.SizeId;
						var sizeName = db.Sizes.Where(t => t.Id == item.SizeId);
						//value to get from string text fopr email
						//using (var reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OrderTemplate.html")))
						//{
						//	body.Append(reader.ReadToEnd());
						//	foreach (var i in sizeName)
						//	{
						//		var sizename = i.SizeName;

						//		body.Replace("{Size}", Convert.ToString(sizename));

						//	}


						orderDetail.TotalMoney = item.Product.PriceSale * item.amount;
						orderDetail.Price = item.Product.PriceSale;
						orderDetail.CreateDate = DateTime.Now;

						//tru di stock có sẵn của product
						Size size = db.Sizes.ToList().Find(t => t.ProductId == item.ProductId);
						size.Stock -= item.amount;
						// cong vao stock da ban duoc cua san pham do
						item.Product.TotalStockSold += item.amount;


						//get size name

						db.OrderDetails.Add(orderDetail);

						//valueProvider for email



					}

					//}

					db.SaveChanges();
					//mail
					//declare body fgor email



					//Xuat thong bao
					//TempData["success"] = "Order Success!";
					//goi mail 
					//using (MailMessage mailMessage = new MailMessage())
					//{
					//	mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
					//	mailMessage.To.Add(new MailAddress(model.Email));
					//	mailMessage.Subject = "Thanks for your order - COCO Store";
					//	mailMessage.Body = body.ToString();
					//	mailMessage.IsBodyHtml = true;
					//	//mailMessage.To.Add(new MailAddress(email.Recipient));
					//	SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
					//	System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
					//	smtpClient.Credentials = credentials;
					//	smtpClient.EnableSsl = true;

					//	smtpClient.Send(mailMessage);
					//}
					//cap nhat thong tin khach hang

					return Redirect(paymentUrl);



				}
				return View();



			}
			else
			{
				if (ModelState.IsValid)
				{
					//Khoi tao don hang
					Order donhang = new Order();
					donhang.CustomerId = model.CustomerId;
					donhang.Address = model.Address;
					donhang.PaymentMethod = model.PaymentMethod;
					donhang.Noti = false;
					donhang.OrderDate = DateTime.Now;
					donhang.TransactStatusId = 1;//Don hang moi
					donhang.Deleted = false;
					donhang.Paid = false;
					donhang.Note = muaHang.Note;
					//donhang.Note = Utilities.StripHTML(model.Note);
					donhang.TotalMoney = Convert.ToInt32(gh.Sum(x => x.TotalMoney));



					db.Orders.Add(donhang);
					db.SaveChanges();


					//tao danh sach don hang

					var viewId = "/OrderView/Details/" + donhang.OrderId;





					foreach (var item in gh)
					{
						OrderDetail orderDetail = new OrderDetail();
						orderDetail.OrderId = donhang.OrderId;
						orderDetail.ProductId = item.Product.Id;
						orderDetail.Amount = item.amount;
						orderDetail.SizeId = item.SizeId;
						var sizeName = db.Sizes.Where(t => t.Id == item.SizeId);
						//value to get from string text fopr email
						using (var reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OrderTemplate.html")))
						{
							body.Append(reader.ReadToEnd());
							foreach (var i in sizeName)
							{
								var sizename = i.SizeName;

								body.Replace("{Size}", Convert.ToString(sizename));

							}


							orderDetail.TotalMoney = item.Product.PriceSale * item.amount;
							orderDetail.Price = item.Product.PriceSale;
							orderDetail.CreateDate = DateTime.Now;

							//tru di stock có sẵn của product
							Size size = db.Sizes.ToList().Find(t => t.ProductId == item.ProductId);
							size.Stock -= item.amount;

							item.Product.TotalStockSold += item.amount;
							//get size name

							db.OrderDetails.Add(orderDetail);

							//valueProvider for email


							body.Replace("{UserName}", donhang.Customer.FullName);
							body.Replace("{Address}", donhang.Customer.Address);
							body.Replace("{PhoneNumber}", donhang.Customer.PhoneNumber);
							body.Replace("{OrderId}", Convert.ToString(donhang.OrderId));
							body.Replace("{Total}", donhang.TotalMoney.ToString("#,##0"));
							body.Replace("{OrderDate}", Convert.ToString(donhang.OrderDate));
							body.Replace("{View}", viewId);

							body.Replace("{ProductName}", item.Product.ProductName);
							body.Replace("{ProductPrice}", (item.Product.PriceSale * item.amount).ToString("#,##0"));
							body.Replace("{ProductForEachPrice}", item.Product.PriceSale.ToString("#,##0"));
							body.Replace("{Quantity}", Convert.ToString(item.amount));

							body.Append("<br />");
						}

					}

					db.SaveChanges();
					//mail
					//declare body fgor email


					//Xuat thong bao
					TempData["success"] = "Order Success!";
					//goi mail 
					using (MailMessage mailMessage = new MailMessage())
					{
						mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
						mailMessage.To.Add(new MailAddress(model.Email));
						mailMessage.Subject = "Thanks for your order - COCO Store";
						mailMessage.Body = body.ToString();
						mailMessage.IsBodyHtml = true;
						//mailMessage.To.Add(new MailAddress(email.Recipient));
						SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
						System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
						smtpClient.Credentials = credentials;
						smtpClient.EnableSsl = true;

						smtpClient.Send(mailMessage);
					}
					//cap nhat thong tin khach hang




					return RedirectToAction("Success");
				}

			}


			return View();


		}


		[Route("dat-hang-thanh-cong.html", Name = "Success")]
		public ActionResult Success()
		{
			try
			{
				var gh = Session["GioHang"] as List<Cart>;
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


				//email
				var body = new StringBuilder();
				var viewId = "/OrderView/Details/" + donhang.OrderId;
				foreach (var item in gh)
				{
					OrderDetail orderDetail = new OrderDetail();
					orderDetail.OrderId = donhang.OrderId;
					orderDetail.ProductId = item.Product.Id;
					orderDetail.Amount = item.amount;
					orderDetail.SizeId = item.SizeId;
					var sizeName = db.Sizes.Where(t => t.Id == item.SizeId);
					using (var reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OrderTemplate.html")))
					{
						body.Append(reader.ReadToEnd());
						foreach (var i in sizeName)
						{
							var sizename = i.SizeName;

							body.Replace("{Size}", Convert.ToString(sizename));

						}


						orderDetail.TotalMoney = item.Product.PriceSale * item.amount;
						orderDetail.Price = item.Product.PriceSale;
						orderDetail.CreateDate = DateTime.Now;

						//tru di stock có sẵn của product
						Size size = db.Sizes.ToList().Find(t => t.ProductId == item.ProductId);
						size.Stock -= item.amount;
						// cong vao stock da ban duoc cua san pham do
						item.Product.TotalStockSold += item.amount;


						//get size name



						//valueProvider for email


						body.Replace("{UserName}", donhang.Customer.FullName);
						body.Replace("{Address}", donhang.Customer.Address);
						body.Replace("{PhoneNumber}", donhang.Customer.PhoneNumber);
						body.Replace("{OrderId}", Convert.ToString(donhang.OrderId));
						body.Replace("{Total}", donhang.TotalMoney.ToString("#,##0"));
						body.Replace("{OrderDate}", Convert.ToString(donhang.OrderDate));
						body.Replace("{View}", viewId);

						body.Replace("{ProductName}", item.Product.ProductName);
						body.Replace("{ProductPrice}", (item.Product.PriceSale * item.amount).ToString("#,##0"));
						body.Replace("{ProductForEachPrice}", item.Product.PriceSale.ToString("#,##0"));
						body.Replace("{Quantity}", Convert.ToString(item.amount));

						body.Append("<br />");
					}
				}
				//send email confirm to user
				if (donhang.PaymentMethod == "VNPAY")
				{
					//save data to vnpay
					if (Request.QueryString.Count > 0)
					{
						string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
						var vnpayData = Request.QueryString;
						PayLib pay = new PayLib();

						//lấy toàn bộ dữ liệu được trả về
						foreach (string s in vnpayData)
						{
							if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
							{
								pay.AddResponseData(s, vnpayData[s]);
							}
						}

						long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
						long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
						string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
						string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

						bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

						if (checkSignature)
						{
							if (vnp_ResponseCode == "00")
							{
								donhang.Paid = true;
								db.SaveChanges();
								//Thanh toán thành công
								using (MailMessage mailMessage = new MailMessage())
								{
									mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
									mailMessage.To.Add(new MailAddress(donhang.Customer.Email));
									mailMessage.Subject = "Thanks for your order - COCO Store";
									mailMessage.Body = body.ToString();
									mailMessage.IsBodyHtml = true;
									//mailMessage.To.Add(new MailAddress(email.Recipient));
									SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
									System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
									smtpClient.Credentials = credentials;
									smtpClient.EnableSsl = true;

									smtpClient.Send(mailMessage);
								}
								TempData["success"] = "Order Success!";
								ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
								HttpContext.Session.Remove("GioHang");
								return View(successVM);

							}
							else
							{
								donhang.TransactStatusId = 5;
								donhang.Paid = false;
								db.SaveChanges();
								//Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
								ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
								TempData["error"] = "Order Failed!";
								return RedirectToAction("Failed", "Checkout");

							}
						}
						else
						{
							donhang.Paid = false;
							donhang.TransactStatusId = 5;
							db.SaveChanges();
							ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
							TempData["error"] = "Order Failed!";
							HttpContext.Session.Remove("GioHang");
							return RedirectToAction("Failed", "Checkout");

						}

						//clear gio hang

					}
				}
				else
				{
					donhang.Paid = false;
					db.SaveChanges();
					TempData["success"] = "Order Success!";
					return View(successVM);
				}
				return View();
			}
			catch
			{
				return View();
			}
		}
		public ActionResult Failed()
		{
			return View();
		}
	}

}