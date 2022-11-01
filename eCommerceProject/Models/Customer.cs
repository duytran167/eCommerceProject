namespace eCommerceProject.Models
{
	public class Customer : ApplicationUser
	{

		public int? LocationId { get; set; }
		public int? District { get; set; }
		public int? Ward { get; set; }

	}
}