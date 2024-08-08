using AcademicPerformance.Models;
using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class DashboardController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public DashboardController(IUnitofwork db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			ViewData["Title"] = "Dashboard";

			var adminCount = await _userManager.GetUsersInRoleAsync("Admin");
			var teacherCount = await _userManager.GetUsersInRoleAsync("Teacher");
			var studentCount = await _userManager.GetUsersInRoleAsync("Student");
			var tpoCount = await _userManager.GetUsersInRoleAsync("TPO");

			ViewData["AdminCount"] = adminCount.Count;
			ViewData["TeacherCount"] = teacherCount.Count;
			ViewData["StudentCount"] = studentCount.Count;
			ViewData["TPOCount"] = tpoCount.Count;

			return View();
		}
	}
}
