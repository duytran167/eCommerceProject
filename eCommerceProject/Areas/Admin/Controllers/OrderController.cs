using ClosedXML.Excel;
using eCommerceProject.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Admin/Order

		public ActionResult Index(string Search_Data, string search, string Filter_Value, int? Page_No)
		{
			if (Search_Data != null)
			{
				Page_No = 1;
			}
			else
			{
				Search_Data = Filter_Value;
			}
			ViewBag.FilterValue = Search_Data;



			var Orders = db.Orders.Include(o => o.Customer).Include(o => o.TransactStatus)
					.AsNoTracking()
					.OrderByDescending(x => x.OrderDate);



			int Size_Of_Page = 20;
			int No_Of_Page = (Page_No ?? 1);
			return View(Orders.ToPagedList(No_Of_Page, Size_Of_Page));
		}
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}

			var order = await db.Orders
					.Include(o => o.Customer)
					.Include(o => o.TransactStatus)
					.FirstOrDefaultAsync(m => m.OrderId == id);

			if (order == null)
			{
				return RedirectToAction("Error", "Admin");
			}

			var Chitietdonhang = db.OrderDetails
					.Include(x => x.Product)
					.AsNoTracking()
					.Where(x => x.OrderId == order.OrderId)
					.OrderBy(x => x.OrderDetailId)
					.ToList();

			ViewBag.ChiTiet = Chitietdonhang;
			return View(order);
		}

		public async Task<ActionResult> ChangeStatus(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}


			var order = await db.Orders
					.AsNoTracking()
					.Include(x => x.Customer)
					.FirstOrDefaultAsync(x => x.OrderId == id);
			if (order == null)
			{
				return RedirectToAction("Error", "Admin");
			}

			ViewData["Trangthai"] = new SelectList(db.TransactStatuses, "Id", "Status", order.TransactStatusId);
			return View(order);
		}

		[HttpPost]
		public async Task<ActionResult> ChangeStatus(int id, Order order)
		{
			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			if (ModelState.IsValid)
			{
				try
				{
					var donhang = await db.Orders.AsNoTracking().Include(x => x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);
					if (donhang != null)
					{
						donhang.Paid = order.Paid;
						donhang.Deleted = order.Deleted;
						donhang.TransactStatusId = order.TransactStatusId;
						if (donhang.Paid == true)
						{
							donhang.PaymentDate = DateTime.Now;
						}
						if (donhang.TransactStatusId == 5) donhang.Deleted = true;
						if (donhang.TransactStatusId == 3) donhang.PackageDate = DateTime.Now;
						if (donhang.TransactStatusId == 4) donhang.ShipDate = DateTime.Now;
					}
					db.Entry(donhang).State = EntityState.Modified;
					db.SaveChanges();
					await db.SaveChangesAsync();
					TempData["success"] = "Change Status Success!";


				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrderExists(order.OrderId))
					{
						return RedirectToAction("Error", "Admin");
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["Trangthai"] = new SelectList(db.TransactStatuses, "Id", "Status", order.TransactStatusId);
			return View(order);
		}
		//export excel
		[HttpPost]
		public FileResult Export()
		{

			DataTable dt = new DataTable("ListOrder");
			dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Id"),
																						new DataColumn("Customer"),
																						new DataColumn("Email"),
																						new DataColumn("DateOrder"),
																						new DataColumn("TotalMoney"),
				new DataColumn("Status")});

			var customers = from customer in db.Orders.Include(t => t.Customer).Include(t => t.TransactStatus).Take(20)
											select customer;

			foreach (var customer in customers)
			{
				dt.Rows.Add(customer.OrderId, customer.Customer.FullName, customer.Customer.Email, customer.OrderDate, customer.TotalMoney, customer.TransactStatus.Status);
			}

			using (XLWorkbook wb = new XLWorkbook())
			{
				wb.Worksheets.Add(dt);
				using (MemoryStream stream = new MemoryStream())
				{
					wb.SaveAs(stream);

					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListOrder.xlsx");
				}
				TempData["success"] = "Export Excel Success!";
			}
		}
		//export single order to pdf
		[HttpPost]
		[ValidateInput(false)]
		public FileResult ExportPDF(string GridHtml)
		{
			using (MemoryStream stream = new System.IO.MemoryStream())
			{
				StringReader sr = new StringReader(GridHtml);
				Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
				PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
				pdfDoc.Open();
				XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
				pdfDoc.Close();
				TempData["success"] = "Export PDF Success!";
				return File(stream.ToArray(), "application/pdf", "Grid.pdf");
			}

		}
		public ActionResult Delete(int id)
		{
			var order = db.Orders.SingleOrDefault(t => t.OrderId == id);
			order.Deleted = true;
			order.OrderDetails.Where(t => t.OrderId == id);
			db.Orders.Remove(order);
			db.SaveChanges();
			TempData["danger"] = "Delete Success!";
			return RedirectToAction(nameof(Index));
		}
		private bool OrderExists(int id)
		{
			return db.Orders.Any(e => e.OrderId == id);
		}
	}

}