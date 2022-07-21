using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace eCommerceProject.Models
{
	public partial class Category
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string CategoryName { get; set; }


		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
	}
}