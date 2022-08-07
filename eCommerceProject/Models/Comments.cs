using System;

namespace eCommerceProject.Models
{
	public class Comments
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public string UserID { get; set; }
		public ApplicationUser User { get; set; }

		public DateTime TimeStamp { get; set; }

		public int RecordID { get; set; }
		public int EntityID { get; set; }
	}

}