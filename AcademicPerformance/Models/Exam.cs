using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models
{
	public class Exam
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[DisplayName("Academic Year")]
		public string  Year { get; set; }

		[Required]
		public string Month { get; set; }
	}
}
