using System.Collections.Generic;
using System.Web.Mvc;

namespace eCommerceProject.ViewModel
{
	public class FilterStatus
	{
		public IEnumerable<SelectListItem> AccountStatus { set; get; }
		public string SelectedStatus { set; get; }
	}
}