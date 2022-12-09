using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Models
{
	public class Contact
	{
		public int Id { get; set; }
		[Required]
		[StringLength(100)]
		[DisplayName("Full Name")]
		public string FullName { get; set; }
		[Required]
		[StringLength(10)]
		[DisplayName("Phone Number")]
		public string PhoneNumber { get; set; }
		[Required]
		[StringLength(50)]
		public string Email { get; set; }
		[Required]
		[StringLength(500)]
		[AllowHtml]
		public string Content { get; set; }
		public DateTime? ArticleDate { get; set; }
		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
		public bool Noti { get; set; }
	}
}