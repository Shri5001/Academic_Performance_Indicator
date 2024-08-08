namespace AcademicPerformance.Models.Data
{
	public class SubjectData
	{
		public string SubjectName { get; set; }
		public string SubjectCode { get; set; }
		public int PresentStudents { get; set; }
		public int PassedStudents { get; set; }
		public int FailedStudents { get; set; }
		public decimal PassingPercentage { get; set; }
		public string Teacher { get; set; }
	}

	public class TopStudentModel
	{
		public string StudentName { get; set; }
		public decimal Percentage { get; set; }
	}

	public class ReportViewModel
	{
		public List<SubjectData> SubjectDataList { get; set; }
		public List<TopStudentModel> TopStudents { get; set; }
	}
}
