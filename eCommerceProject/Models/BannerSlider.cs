using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace eCommerceProject.Models
{
	public class BannerSlider
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		public string Description { get; set; }

		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
	}
}