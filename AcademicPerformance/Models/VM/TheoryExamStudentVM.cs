using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class TheoryExamStudentVM
	{
		public Exam Exam { get; set; }
		public Branch Branch { get; set; }
		public Subject Subject { get; set; }
		public IEnumerable<SelectListItem> Students { get; set; }
		public TheoryMarks TheoryMarks { get; set; }
		public ExamSubject ExamSubject { get; set; }
		public List<TheoryMarks> TheoryMarksList { get; set; }
	}
}
