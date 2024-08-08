using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class TeacherBranchVM
	{
		public IEnumerable<SelectListItem> Branches { get; set; }
		public Teacher Teacher { get; set; }

		public ApplicationUser User { get; set; }

		public int SelectedBranchId { get; set; }
	}
}
