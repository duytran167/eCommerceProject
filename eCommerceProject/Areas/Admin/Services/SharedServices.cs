using eCommerceProject.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace eCommerceProject.Areas.Admin.Services
{
	public class SharedServices
	{
		public bool LeaveComment(Comments comment)
		{
			ApplicationDbContext context = new ApplicationDbContext();
			context.Comments.Add(comment);

			return context.SaveChanges() > 0;


		}
		public List<Comments> GetComments(int entityID, int recordID)
		{
			ApplicationDbContext context = new ApplicationDbContext();
			//var a = User.Identity.GetUserId();
			return context.Comments.Include(c => c.User).Where(x => x.EntityID == entityID && x.RecordID == recordID).ToList();




		}
	}
}