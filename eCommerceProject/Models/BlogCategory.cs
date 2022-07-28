using System.ComponentModel.DataAnnotations;

namespace eCommerceProject.Models
{
	public class BlogCategory
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string CategoryName { get; set; }
	}
}