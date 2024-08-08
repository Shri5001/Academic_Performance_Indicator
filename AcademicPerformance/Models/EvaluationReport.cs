using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AcademicPerformance.Models
{
	public class EvaluationReport
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		[Required]
		[DisplayName("Branch/Department")]
		public int BranchId { get; set; }

		[ForeignKey("BranchId")]
		public Branch Branch { get; set; }

		[Required]
		[DisplayName("Subject")]
		public int SubjectId { get; set; }

		[ForeignKey("SubjectId")]
		public Subject Subject { get; set; }

		[Required]
		[DisplayName("Academic Year")]
		public string Year { get; set; }

		[Required]
		[DisplayName("Branch/Department Year")]
		public int BranchYear { get; set; }

		[Required]
		[DisplayName("Semester")]
		public int Sem { get; set; }

		public string ReportData { get; set; }
	}
}
