using eCommerceProject.Models;
using System.Collections.Generic;

namespace eCommerceProject.ViewModel
{
	public class OrderViewModel
	{
		public Order Orders { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
	}
}