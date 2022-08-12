using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace eCommerceProject.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string ProductName { get; set; }
		public string ShortDesc { get; set; }
		[AllowHtml]
		public string Description { get; set; }

		[DisplayName("Upload File")]

		public virtual ICollection<ImageProduct> ImageProducts { get; set; }

		public int CategoryID { get; set; }
		public Category Category { get; set; }
		public int Price { get; set; }
		public int Discount { get; set; }
		public int UnitsInStock { get; set; }
		public bool BestSellers { get; set; }
		public bool Active { get; set; }
		public DateTime CreatedDate { get; set; }
		public Product()
		{
			CreatedDate = DateTime.Now;
		}
	}
}