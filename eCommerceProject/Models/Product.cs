﻿using System;
using System.Collections.Generic;
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

		public virtual ICollection<ImageProduct> ImageProducts { get; set; }
		//[DisplayName("Upload File")]
		//public string ImagePath { get; set; }

		//[NotMapped]
		//public HttpPostedFileBase ImageFile { get; set; }
		public int SizeID { get; set; }
		public Size Size { get; set; }

		public int CategoriesID { get; set; }
		public Category Categories { get; set; }
		public int Price { get; set; }
		public int Discount { get; set; }
		public string ProductCode { get; set; }
		public bool BestSellers { get; set; }
		public bool Active { get; set; }
		public DateTime CreatedDate { get; set; }
		public Product()
		{
			CreatedDate = DateTime.Now;
		}
	}
}