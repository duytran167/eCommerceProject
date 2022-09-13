using System.ComponentModel.DataAnnotations;

namespace eCommerceProject.Models
{
	public class SendEmail
	{
		public int Id { get; set; }
		[Required]
		//[RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "Invalid email")]
		[DataType(DataType.MultilineText)]
		public string CustomerEmail { get; set; }



		public string subject { get; set; }
		[Required]
		public string body { get; set; }


	}
}