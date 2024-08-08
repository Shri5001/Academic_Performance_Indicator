namespace AcademicPerformance.Models.Data
{
	public class TheoryData
	{
		public string StudentId { get; set; }
		public string SeatNumber { get; set; }
		public int InSem { get; set; }
		public int EndSem { get; set; }
		public int TermWork { get; set; }
		public List<int> ExperimentMarks { get; set; }
		public int Total480 { get; set; }
		public float Total25 { get; set; }
		public float Marks { get; set; }
		public int Total { get; set; }
	}
}
