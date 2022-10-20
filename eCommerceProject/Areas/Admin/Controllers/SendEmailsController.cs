using eCommerceProject.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SendEmailsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/SendEmails
		public ActionResult Index()
		{
			return View(db.SendEmails.ToList());
		}

		// GET: Admin/SendEmails/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			SendEmail sendEmail = db.SendEmails.Find(id);
			if (sendEmail == null)
			{
				return HttpNotFound();
			}
			return View(sendEmail);
		}

		// GET: Admin/SendEmails/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/SendEmails/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(SendEmail sendEmail)
		{
			//string path = "~/Content/ImageEmail/" + Guid.NewGuid() + "/";
			if (ModelState.IsValid)
			{

				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
				var ToMuliId = sendEmail.CustomerEmail.Split(',', ';');
				foreach (var ToEMailId in ToMuliId)
				{
					mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
				}

				//mailMessage.CC.Add(new MailAddress("solutions@yogihosting.com"));

				string body = string.Empty;
				using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/Template.html")))
				{
					body = reader.ReadToEnd();
				}
				mailMessage.Body = body.Replace("{Description}", sendEmail.body);
				mailMessage.Subject = sendEmail.subject;

				mailMessage.IsBodyHtml = true;

				//if (sendEmail.file[0] != null)
				//{
				//	Directory.CreateDirectory(Server.MapPath(path));
				//	foreach (HttpPostedFileBase file in sendEmail.file)
				//	{
				//		string filePath = Server.MapPath(path + file.FileName);
				//		file.SaveAs(filePath);

				//		mailMessage.Attachments.Add(new Attachment(Server.MapPath(path + file.FileName)));
				//	}
				//}

				SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
				System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
				smtpClient.Credentials = credentials;
				smtpClient.EnableSsl = true;
				//save db
				var newMail = new SendEmail()
				{
					CustomerEmail = sendEmail.CustomerEmail,
					subject = sendEmail.subject,
					body = sendEmail.body,

				};

				db.SendEmails.Add(newMail);
				db.SaveChanges();
				ViewBag.Result = "Mail Sent!";

				//send mail
				//await UserManager.SendEmailAsync(allUserCustomer., "New From COCO Store", "Some thing news.... <br> out now \"" + newBlog.Title + "\"  was submited. Thanks for your submited Ideas, pls wait for QA respond!");

				try
				{
					smtpClient.Send(mailMessage);
					TempData["success"] = "Send Success!";
				}
				catch (Exception ex)
				{
					ViewBag.Result = ex.Message;
					TempData["danger"] = "Send Failed!";
				}
			}

			return RedirectToAction("Index");
		}

		// GET: Admin/SendEmails/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			SendEmail sendEmail = db.SendEmails.Find(id);
			if (sendEmail == null)
			{
				return HttpNotFound();
			}
			return View(sendEmail);
		}

		// POST: Admin/SendEmails/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,CustomerID,subject,body")] SendEmail sendEmail)
		{
			if (ModelState.IsValid)
			{
				db.Entry(sendEmail).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(sendEmail);
		}

		// GET: Admin/SendEmails/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			SendEmail sendEmail = db.SendEmails.Find(id);
			if (sendEmail == null)
			{
				return HttpNotFound();
			}
			return View(sendEmail);
		}

		// POST: Admin/SendEmails/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			SendEmail sendEmail = db.SendEmails.Find(id);
			db.SendEmails.Remove(sendEmail);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
