using eCommerceProject.Models;
using System.Collections.Generic;

namespace eCommerceProject.ViewModel
{
	public class BlogVM
	{
		public BlogPost BlogPost { get; set; }
		public int Id { get; set; }
		public IEnumerable<BlogCategory> BlogCategories { get; set; }

		public List<Comments> Comments { get; set; }
		public int EntityID { get; set; }
		public string UserName { get; set; }

	}
	public class CommentViewModels
	{
		public int CommentID { get; set; }
		public string Text { get; set; }

		public int RecordID { get; set; }

		public int EntityID { get; set; }

	}
}