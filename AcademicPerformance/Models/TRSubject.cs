using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class TRSubject
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[DisplayName("Subject")]
		public int SubjectId { get; set; }

		[ForeignKey("SubjectId")]
		public Subject Subject { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		[DisplayName("No. of Students Appeared")]
		public int StudentsAppeared { get; set; }

		[Required]
		[DisplayName("No. of Students Passed (All Clear)")]
		public int StudentsPass { get; set; }

		[Required]
		[DisplayName("No. of Students Failed")]
		public int StudentsFailed { get; set; }

		[Required]
		[DisplayName("No. of Students with Distinctions")]
		public int Distinction { get; set; }

		[Required]
		[DisplayName("No. of Students with First Class")]
		public int FirstClass { get; set; }

		[Required]
		[DisplayName("No. of Students with Higher Second Class")]
		public int HigherSecondClass { get; set; }

		[Required]
		[DisplayName("% of Passing")]
		public int PercentagePassing { get; set; }

		[Required]
		[DisplayName("Teacher")]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
	}
}
