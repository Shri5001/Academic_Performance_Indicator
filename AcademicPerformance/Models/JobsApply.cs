using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class JobsApply
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("StudentId")]
		[DisplayName("Student")]
		public int StudentId { get; set; }
		public Student Student { get; set; }

		[Required]
		[ForeignKey("JobsId")]
		[DisplayName("Job")]
		public int JobsId { get; set; }
		public Jobs Jobs { get; set; }
	}
}
