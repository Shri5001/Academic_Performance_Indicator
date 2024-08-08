using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models.VM
{
	public class GenerateReportGetVM
	{
		public IEnumerable<SelectListItem> Branches { get; set; }

		[Required]
		[DisplayName("Branch")]
		public int SelectedBranchId { get; set; }

		[Required]
		[DisplayName("Academic Year")]
		public string Year { get; set; }

		[Required]
		[DisplayName("Year")]
		public int BranchYear { get; set; }

		[Required]
		[DisplayName("Semester")]
		public int Sem { get; set; }
	}
}
