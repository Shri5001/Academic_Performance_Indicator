using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models.VM
{
	public class StudentImageVM
	{
		public Student Student { get; set; }

		[Required(ErrorMessage = "Please choose profile image")]
		[Display(Name = "Profile Picture")]
		public IFormFile Image { get; set; }
	}
}
