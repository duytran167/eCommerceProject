using eCommerceProject.Models;
using System.ComponentModel.DataAnnotations;

namespace eCommerceProject.ViewModel
{
	public class Cart
	{
		public int ProductId { get; set; }
		public Product Product { get; set; }
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public int amount { get; set; }
		public int SizeId { get; set; }
		public Size Size { get; set; }

		public double TotalMoney => amount * Product.PriceSale;

	}
}