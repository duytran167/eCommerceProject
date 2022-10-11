using eCommerceProject.Models;

namespace eCommerceProject.ViewModel
{
	public class CartItem
	{
		public Product Product { get; set; }
		public int amount { get; set; }
		public double TotalMoney => amount * Product.PriceSale;
		public Size Size { gets; set; }
	}
}