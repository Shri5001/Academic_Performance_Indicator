using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class TheoryMarks
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("StudentId")]
		[DisplayName("Student")]
		public int StudentId { get; set; }
		public Student Student { get; set; }

		[Required]
		[ForeignKey("SubjectId")]
		[DisplayName("Subject")]
		public int SubjectId { get; set; }
		public Subject Subject { get; set; }

		[Required]
		[ForeignKey("BranchId")]
		public int BranchId { get; set; }
		public Branch Branch { get; set; }

		[Required]
		[ForeignKey("ExamId")]
		public int ExamId { get; set; }
		public Exam Exam { get; set; }

		[Required]
		[DisplayName("Seat No.")]
		public string SeatNo { get; set; }

		[Required]
		public string Attendance { get; set; }

		[Required]
		[DisplayName("In-Sem")]
		public int InSem { get; set; }

		[Required]
		[DisplayName("End-Sem")]
		public int EndSem { get; set; }

		[Required]
		public int Total { get; set; }

		[Required]
		public int Status { get; set; }
	}
}
