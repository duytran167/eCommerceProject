using eCommerceProject.Models;
using System.Collections.Generic;

namespace eCommerceProject.ViewModel
{
	public class ProductVM
	{
		public Product Product { get; set; }
		public int Id { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public ImageProduct ImageProduct { get; set; }

		public Size Size { get; set; }
		public virtual ICollection<ImageProduct> ImageProducts { get; set; }
		public IList<Size> Sizes { get; set; }
		//public List<Comments> Comments { get; set; }
		//public int EntityID { get; set; }
		//public string UserName { get; set; }
		public ProductVM()
		{

		}

	}
}