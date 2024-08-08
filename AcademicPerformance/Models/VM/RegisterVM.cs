using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class RegisterVM
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string? Password { get; set; }

		//[DataType(DataType.Password)]
		//[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		//public string ConfirmPassword { get; set; }

		public string FullName { get; set; }

		[Required(ErrorMessage = "Please select a role")]
		public string SelectedRole { get; set; }

		public IEnumerable<SelectListItem>? AvailableRoles { get; set; }
	}
}
