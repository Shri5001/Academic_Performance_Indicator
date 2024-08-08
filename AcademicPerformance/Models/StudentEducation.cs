using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AcademicPerformance.Models
{
	public class StudentEducation
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		[Required]
		[DisplayName("Institute/University with Location")]
		public string Institute { get; set; }

		[Required]
		[DisplayName("Year (From-To)")]
		public string Year { get; set; }

		[Required]
		[DisplayName("Degree")]
		public string Degree { get; set; }

		[Required]
		[DisplayName("Percentage/GPA")]
		public string Percentage { get; set; }

		[DisplayName("Document")]
		public string FileUrl { get; set; }

		[NotMapped] // Exclude this property from database mapping
		[DisplayName("Upload Document")]
		public IFormFile DocumentFile { get; set; }
	}
}
