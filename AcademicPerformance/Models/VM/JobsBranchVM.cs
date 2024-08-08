using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class JobsBranchVM
	{
		public IEnumerable<SelectListItem> Branches { get; set; }
		public Jobs Jobs { get; set; }
	}
}
