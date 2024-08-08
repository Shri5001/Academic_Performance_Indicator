using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class PracticalMarks
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
		[DisplayName("Obtained from 480")]
		public int Total480 { get; set; }

		[Required]
		[DisplayName("Term Work")]
		public float TermWork { get; set; }

		[Required]
		[DisplayName("External Practical")]
		public int ExternalPractical { get; set; }

		[Required]
		public float Total { get; set; }

		[Required]
		public int Status { get; set; }

		[Required]
		[DisplayName("Experiment Marks")]
		public string ExperimentMarks { get; set; }
	}
}
