namespace AcademicPerformance.Models.Data
{
	public class SubjectStatistics
	{
		public Subject? Subject { get; set; }
		public int PresentStudents { get; set; }
		public int PassedStudents { get; set; }
		public int FailedStudents { get; set; }
		public int AtktStudents { get; set; }
		public int DistinctionStudents { get; set; }
		public int FirstClassStudents { get; set; }
		public int SecondClassStudents { get; set; }
		public float PassingPercentage { get; set; }
		public ApplicationUser Teacher { get; set; }
	}
}
