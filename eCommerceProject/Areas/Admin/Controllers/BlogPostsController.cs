using eCommerceProject.Areas.Admin.Services;
using eCommerceProject.Enums;
using eCommerceProject.Models;
using eCommerceProject.ViewModel;
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
		SharedServices service = new SharedServices();
		// GET: BlogPosts
		//public ActionResult Index(int? page, string search, int? filter, int CatID = 0)
		//{

		//	var blogPost = from s in db.BlogPosts
		//								 .Include(t => t.User)
		//								 .Include(t => t.BlogCategories)
		//								 .OrderByDescending(t => t.CreateDate).ToList()
		//								 select s;
		//	if (CatID != 0)
		//	{
		//		blogPost = db.BlogPosts.AsNoTracking()
		//			.Include(x => x.BlogCategories)
		//			.Where(x => x.CategoryID == CatID)
		//			.OrderByDescending(x => x.Id).ToList();
		//	}
		//	else
		//	{
		//		blogPost = db.BlogPosts.AsNoTracking()
		//			.Include(x => x.BlogCategories)
		//			.OrderByDescending(x => x.Id).ToList();
		//	}
		//	if (!String.IsNullOrWhiteSpace(search))
		//	{
		//		blogPost = db.BlogPosts.Where(s => s.Title.Contains(search)
		//													 || s.ShortContents.Contains(search)).ToList();
		//	}

		//	ViewData["DanhMuc"] = new SelectList(db.BlogCategories, "Id", "CategoryName", CatID);

		//	return View(blogPost.ToPagedList(page ?? 1, 5));
		//}
		public ActionResult Index(int? page, string search, int? filter)
		{

			var blogPost = from s in db.BlogPosts.Include(t => t.User).Include(x => x.BlogCategories).AsNoTracking().OrderByDescending(t => t.CreateDate).ToList()
										 select s;

			if (!String.IsNullOrWhiteSpace(search))
			{
				blogPost = blogPost.Where(s => s.Title.Contains(search)
															 || s.ShortContents.Contains(search)).ToList();
			}
			if (!String.IsNullOrWhiteSpace(filter.ToString()))
			{
				//Filter results based on company selected.

				blogPost = blogPost.Where(x => x.BlogCategoriesID.Equals(filter)).ToList();


			}
			ViewData["DanhMuc"] = new SelectList(db.BlogCategories, "Id", "CategoryName");


			return View(blogPost.ToPagedList(page ?? 1, 5));
		}


		// GET: BlogPosts/Details/5
		public ActionResult Details(int? id)
		{

			if (id == null)
			{
				return RedirectToAction("Error", "Admin");
			}


			var course = new ViewModel.BlogVM();
			BlogPost update = db.BlogPosts.ToList().Find(u => u.Id == id);
			update.BlogPostView += 1;
			db.SaveChanges();
			//


			// count comment
			var cmtCount = db.Comments.Where(t => t.RecordID == update.Id).ToList();
			ViewBag.CmtCount = cmtCount.Count();

			var a = User.Identity.GetUserId();
			course.EntityID = (int)EntityEnums.Post;
			course.BlogPost = db.BlogPosts.
				Include(i => i.User).
			SingleOrDefault(t => t.Id == id);
			course.Comments = service.GetComments((int)EntityEnums.Post, course.BlogPost.Id);
			course.UserName = User.Identity.GetUserId();


			if (course == null)
			{
				return RedirectToAction("Error", "Admin");
			}

			return View(course);
		}

		// GET: BlogPosts/Create
		[HttpGet]
		public ActionResult Create()
		{
			var post = new ViewModel.BlogVM()
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
		public ActionResult Create(ViewModel.BlogVM model)
		{

			if (ModelState.IsValid)
			{
				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
				string fileName = Path.GetFileNameWithoutExtension(model.BlogPost.ImageFile.FileName);

				string exe = Path.GetExtension(model.BlogPost.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				model.BlogPost.ImagePath = "~/Content/ImageProduct/ImageBlog/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileName);
				model.BlogPost.ImageFile.SaveAs(fileName);


				var userId = User.Identity.GetUserId();

				var newBlog = new BlogPost()
				{
					Title = model.BlogPost.Title,
					BlogCategoriesID = model.Id,
					ShortContents = model.BlogPost.ShortContents,
					Contents = model.BlogPost.Contents,
					ImagePath = model.BlogPost.ImagePath,
					isHotNews = model.BlogPost.isHotNews,
					isPublic = model.BlogPost.isPublic,
					CreateDate = model.BlogPost.CreateDate,
					UserId = userId
				};

				db.BlogPosts.Add(newBlog);
				db.SaveChanges();
				TempData["success"] = "Create Success!";
				return RedirectToAction("Index");
			}

			return View(model);
		}

		// GET: BlogPosts/Edit/5
		public ActionResult Edit(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var course = db.BlogPosts.SingleOrDefault(t => t.Id == id);


			var blogPost = new ViewModel.BlogVM()
			{
				Id = id,
				BlogPost = course,
				BlogCategories = db.BlogCategories.ToList()
			};

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
		public ActionResult Edit(ViewModel.BlogVM model)
		{
			if (ModelState.IsValid)
			{

				var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));

				string fileName = Path.GetFileNameWithoutExtension(model.BlogPost.ImageFile.FileName);

				string exe = Path.GetExtension(model.BlogPost.ImageFile.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssfff") + exe;
				model.BlogPost.ImagePath = "~/Content/ImageProduct/ImageBlog/" + fileName;
				fileName = Path.Combine(Server.MapPath("~/Content/ImageProduct/ImageBlog/"), fileName);
				model.BlogPost.ImageFile.SaveAs(fileName);


				//get user id
				var userId = User.Identity.GetUserId();
				var post = db.BlogPosts.SingleOrDefault(t => t.Id == model.Id);

				post.Title = model.BlogPost.Title;
				post.ShortContents = model.BlogPost.ShortContents;
				post.ImagePath = model.BlogPost.ImagePath;
				post.BlogCategoriesID = model.BlogPost.BlogCategoriesID;
				post.Contents = model.BlogPost.Contents;

				post.isHotNews = model.BlogPost.isHotNews;
				post.isPublic = model.BlogPost.isPublic;
				//post.View = viewModel.View;

				db.SaveChanges();
				TempData["success"] = "Edit Success!";
				return RedirectToAction("Index");
			}
			return View(model);
		}
		public ActionResult Delete(int id)
		{
			var removeBlog = db.BlogPosts.SingleOrDefault(t => t.Id == id);
			db.BlogPosts.Remove(removeBlog);
			db.SaveChanges();

			return RedirectToAction("Index");
			TempData["success"] = "Delete Success!";
		}
		// Filter and Search
		public ActionResult Filter(int CatID = 0)
		{

			var url = $"/Admin/BlogPosts?CatID={CatID}";
			if (CatID == 0)
			{
				url = $"/Admin/BlogPosts";
			}
			return Json(new { status = "success", redirectUrl = url });
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
		//comments
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public JsonResult LeaveComment(CommentViewModels model)
		{
			JsonResult result = new JsonResult();


			try
			{
				var comment = new Comments();
				comment.Id = model.CommentID;
				comment.Text = model.Text;
				comment.RecordID = model.RecordID;
				comment.EntityID = model.EntityID;
				comment.UserID = User.Identity.GetUserId();
				comment.TimeStamp = DateTime.Now;

				var res = service.LeaveComment(comment);
				result.Data = new { Success = res };
			}
			catch (Exception ex)
			{
				result.Data = new { Success = false, Message = ex.Message };
			}

			return result;
		}

		public ActionResult DeleteComment(int id)
		{
			BlogPost update = db.BlogPosts.ToList().Find(u => u.Id == id);
			if (ModelState.IsValid)
			{
				//string currentUserId = User.Identity.GetUserId();
				var removeCmt = db.Comments
					//.Where(x => x.UserID == currentUserId)
					.SingleOrDefault(t => t.Id == id);
				db.Comments.Remove(removeCmt);
				db.SaveChanges();
				return RedirectToAction("Details", "", new { id = removeCmt.Id });
			}

			return View(id);
		}
	}
}
