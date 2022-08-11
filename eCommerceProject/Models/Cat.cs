using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace eCommerceProject.Models
{
	public class Cat
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }

		[NotMapped]

		public List<HttpPostedFileBase> ImageFile { get; set; }
	}
}