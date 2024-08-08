using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class ExamSubject
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		[Required]
		[ForeignKey("BranchId")]
		public int BranchId { get; set; }
		public Branch Branch { get; set; }

		[Required]
		[ForeignKey("ExamId")]
		public int ExamId { get; set; }
		public Exam Exam { get; set; }

		[Required]
		[ForeignKey("SubjectId")]
		[DisplayName("Subject")]
		public int SubjectId { get; set; }
		public Subject Subject { get; set; }

		[Required]
		[DisplayName("Branch/Department Year")]
		public int Year { get; set; }

		[Required]
		[DisplayName("Semester")]
		public int Sem { get; set; }
	}
}
