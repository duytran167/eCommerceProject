using System.ComponentModel.DataAnnotations;

namespace eCommerceProject.Models
{
	public class Size
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string SizeName { get; set; }
		public int Stock { get; set; }

		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
	}
}