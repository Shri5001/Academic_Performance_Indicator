using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class TeacherSubjectVM
	{
		public IEnumerable<SelectListItem> Subjects { get; set; }
		public TeacherSubject Teacher { get; set; }

		public ApplicationUser User { get; set; }

		public int SelectedSubjectId { get; set; }
	}
}
