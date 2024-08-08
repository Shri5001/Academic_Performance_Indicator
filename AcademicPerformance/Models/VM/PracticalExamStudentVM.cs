using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class PracticalExamStudentVM
	{
		public Exam Exam { get; set; }
		public Branch Branch { get; set; }
		public Subject Subject { get; set; }
		public IEnumerable<SelectListItem> Students { get; set; }
		public PracticalMarks PracticalMarks { get; set; }
		public ExamSubject ExamSubject { get; set; }

		public List<ExperimentMarksVM> ExperimentMarks { get; set; }

		public List<PracticalMarks>? PraticalMarksList { get; set; }
	}

	public class ExperimentMarksVM
	{
		public string ExperimentName { get; set; }
		public List<SubsectionMarksVM> SubsectionMarks { get; set; }
	}

	public class SubsectionMarksVM
	{
		public string SubsectionName { get; set; }
		public int Marks { get; set; }
	}
}
