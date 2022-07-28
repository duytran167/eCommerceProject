using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class BlogPostsController : Controller
	{
		private ApplicationDbContext _context;
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: BlogPosts
		public ActionResult Index(int? page, string search, int? filter)
		{

			var blogPost = from s in db.BlogPosts.Include(t => t.User).AsNoTracking().OrderByDescending(t => t.CreateDate).ToList()
										 select s;

			if (!String.IsNullOrWhiteSpace(search))
			{
				blogPost = db.BlogPosts.Where(s => s.Title.Contains(search)
															 || s.ShortContents.Contains(search)).ToList();
			}



			return View(blogPost.ToPagedList(page ?? 1, 5));
		}

		// GET: BlogPosts/Details/5
		public ActionResult Details(int? id)
		{

			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}
			BlogPost blogPost = db.BlogPosts.Include(t => t.User).SingleOrDefault(t => t.Id == id);
			if (blogPost == null)
			{
				return RedirectToAction("Error", "Admin");
			}

			return View(blogPost);
		}

		// GET: BlogPosts/Create
		public ActionResult Create()
		{
			var post = new Models.BlogPost()
			{
				BlogCategories = db.BlogCategories.ToList(),
			};

			return View(post);
		}

		// POST: BlogPosts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create([Bind(Include = "Id,Title,ShortContents,Contents,ImageFile,isPublic,isHotNews,CreateDate,UserId,CategoryID")] BlogPost blogPost)
		{
			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(blogPost.ImageFile.FileName);
				string exe = Path.GetExtension(blogPost.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				blogPost.ImagePath = "~/Content/ImageProduct/ImageBlog/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileName);
				blogPost.ImageFile.SaveAs(fileName);

				var userId = User.Identity.GetUserId();

				var newBlog = new BlogPost()
				{
					Title = blogPost.Title,
					CategoryID = blogPost.CategoryID,
					ShortContents = blogPost.ShortContents,
					Contents = blogPost.Contents,
					ImagePath = blogPost.ImagePath,
					isHotNews = blogPost.isHotNews,
					isPublic = blogPost.isPublic,
					CreateDate = blogPost.CreateDate,
					UserId = userId
				};

				db.BlogPosts.Add(newBlog);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Index");
			}

			return View(blogPost);
		}

		// GET: BlogPosts/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			BlogPost blogPost = db.BlogPosts.Find(id);
			if (blogPost == null)
			{
				return HttpNotFound();
			}
			return View(blogPost);
		}

		// POST: BlogPosts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Title,ShortContents,Contents,ImageFile,isPublic,isHotNews,CreateDate,UserId")] BlogPost blogPost)
		{
			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(blogPost.ImageFile.FileName);
				string exe = Path.GetExtension(blogPost.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				blogPost.ImagePath = "~/Content/ImageProduct/ImageBlog/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileName);
				blogPost.ImageFile.SaveAs(fileName);
				//get user id
				var userId = User.Identity.GetUserId();
				var post = db.BlogPosts.FirstOrDefault(t => t.Id == blogPost.Id);
				post.Title = blogPost.Title;
				post.ShortContents = blogPost.ShortContents;
				post.ImagePath = blogPost.ImagePath;
				post.CategoryID = blogPost.CategoryID;
				post.Contents = blogPost.Contents;

				post.isHotNews = blogPost.isHotNews;
				post.isPublic = blogPost.isPublic;
				//post.View = viewModel.View;

				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Index");
			}
			return View(blogPost);
		}
		public ActionResult Delete(int id)
		{
			var removeBlog = db.BlogPosts.SingleOrDefault(t => t.Id == id);
			db.BlogPosts.Remove(removeBlog);
			db.SaveChanges();

			return RedirectToAction("Index");
			TempData["success"] = "Delete Success!";
		}

		// GET: BlogPosts/Delete/5
		//public ActionResult Delete(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	BlogPost blogPost = db.BlogPosts.Find(id);
		//	if (blogPost == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(blogPost);
		//}

		//// POST: BlogPosts/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(int id)
		//{
		//	BlogPost blogPost = db.BlogPosts.Find(id);
		//	db.BlogPosts.Remove(blogPost);
		//	db.SaveChanges();
		//	return RedirectToAction("Index");
		//}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
		//post image in blogpost
		[HttpPost]
		public JsonResult UploadFile(HttpPostedFileBase uploadedFiles)
		{
			string returnImagePath = string.Empty;
			string fileName;
			string Extension;
			string imageName;
			string imageSavePath;

			if (uploadedFiles.ContentLength > 0)
			{
				fileName = Path.GetFileNameWithoutExtension(uploadedFiles.FileName);
				Extension = Path.GetExtension(uploadedFiles.FileName);
				imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
				imageSavePath = Server.MapPath("/Content/ImageProduct/ImageBlog/") + imageName +
Extension;

				uploadedFiles.SaveAs(imageSavePath);
				returnImagePath = "/Content/ImageProduct/ImageBlog/" + imageName +
Extension;
			}

			return Json(Convert.ToString(returnImagePath), JsonRequestBehavior.AllowGet);
		}
	}
}
