using AcademicPerformance.Models.Data;

namespace AcademicPerformance.Models.VM
{
	public class GeneratedTheoryReportVM
	{
		public List<Subject> Subjects { get; set; }
		public List<TheoryResult> TheoryResults { get; set; }

		public List<TheoryData> TheoryDatas { get; set; }
	}
}
