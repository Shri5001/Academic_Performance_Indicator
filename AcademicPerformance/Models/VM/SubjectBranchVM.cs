using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class SubjectBranchVM
	{
		public IEnumerable<SelectListItem> Branches{ get; set; }
		public Subject Subject { get; set; }
	}
}
