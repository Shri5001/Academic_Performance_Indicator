using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models.VM
{
	public class StudentBarnchVM
	{
		public IEnumerable<SelectListItem> Branches { get; set; }
		public Student Student { get; set; }

		public ApplicationUser User { get; set; }

		public int SelectedBranchId { get; set; }

		[Display(Name = "Profile Picture")]
		public IFormFile Image { get; set; }
	}
}
