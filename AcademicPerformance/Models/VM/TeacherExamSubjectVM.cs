using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class TeacherExamSubjectVM
	{
		public Exam Exam { get; set; }
		public IEnumerable<SelectListItem> Subjects { get; set; }
		public ApplicationUser User { get; set; }
		public ExamSubject ExamSubject { get; set; }
		public int BranchId { get; set; }
	}
}
