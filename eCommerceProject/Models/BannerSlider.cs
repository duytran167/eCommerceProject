using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace eCommerceProject.Models
{
	public partial class BannerSlider
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		public string Description { get; set; }
		public DateTime? ArticleDate { get; set; }
		public BannerSlider()
		{
			ArticleDate = DateTime.Now;
		}

		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }

	}
}