using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eCommerceProject.Models
{
	public class StarRatingAndComment
	{
		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Comments { get; set; }

		public DateTime ThisDateTime { get; set; }
		public int Rating { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		[DisplayName("Public")]
		public bool isPublic { get; set; }

		public StarRatingAndComment()
		{
			ThisDateTime = DateTime.Now;

		}
	}
}