using System;

namespace eCommerceProject.Models
{
	public class ImageProduct
	{
		public Guid Id { get; set; }

		//[DisplayName("Upload File")]
		//public string ImagePath { get; set; }

		//[NotMapped]
		//public HttpPostedFileBase ImageFile { get; set; }
		public string FileName { get; set; }
		public string Extension { get; set; }
		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
	}
}