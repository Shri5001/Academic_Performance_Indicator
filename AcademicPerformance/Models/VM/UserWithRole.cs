using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Models.VM
{
	public class UserWithRole
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public string Password { get; set; }

		public IEnumerable<SelectListItem> AvailableRoles { get; set; }
	}
}
