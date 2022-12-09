using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Models
{
	public partial class BlogPost
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
		public string Title { get; set; }
		[StringLength(12255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
		public string ShortContents { get; set; }
		[AllowHtml]
		public string Contents { get; set; }

		[DisplayName("Upload File")]

		public string ImagePath { get; set; }

		[NotMapped]

		public HttpPostedFileBase ImageFile { get; set; }
		[DisplayName("Public")]
		public bool isPublic { get; set; }
		[DisplayName("Hot News")]
		public bool isHotNews { get; set; }

		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime CreateDate { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public int BlogCategoriesID { get; set; }
		public BlogCategory BlogCategories { get; set; }
		public int BlogPostView { get; set; }

		public BlogPost()
		{
			CreateDate = DateTime.Now;
		}
	}
}