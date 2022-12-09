using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace eCommerceProject.Models
{
	public partial class Collection
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string Name { get; set; }
		[AllowHtml]
		public string Description { get; set; }
		public DateTime? ArticleDate { get; set; }
		public Collection()
		{
			ArticleDate = DateTime.Now;
		}

		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
	}
}