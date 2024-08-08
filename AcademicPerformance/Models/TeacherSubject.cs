using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models
{
	public class TeacherSubject
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		[Required]
		public int SubjectId { get; set; }

		[ForeignKey("SubjectId")]
		public Subject Subject { get; set; }
	}
}
