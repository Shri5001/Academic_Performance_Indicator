using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class EVReportBranchVM
	{
		public IEnumerable<SelectListItem> Branches { get; set; }
		public IEnumerable<SelectListItem> Subjects { get; set; }
		public ApplicationUser User { get; set; }

		public EvaluationReport EVReport { get; set; }

		public int SelectedBranchId { get; set; }
		public int SelectedSubjectId { get; set; }
	}
}
