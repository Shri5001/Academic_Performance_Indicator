using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AcademicPerformance.Models.ServiceFilter
{
	public class CheckStudentProfileFilter : IAsyncActionFilter
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public CheckStudentProfileFilter(IUnitofwork db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var user = await _userManager.GetUserAsync(context.HttpContext.User);
			var userId = user?.Id;

			var student = _db.Student.Get(u => u.UserId == userId);

			if (student == null || string.IsNullOrEmpty(student.Nationality) || student.RollNo == null || student.DOB == null)
			{
				context.Result = new RedirectToActionResult("Index", "Profile", new { area = "Student" });
				return;
			}

			await next();
		}
	}
}
