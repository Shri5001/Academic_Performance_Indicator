using AcademicPerformance.Models;
using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AcademicPerformance.Areas.Student.Controllers
{
	[Area("Student")]
	[Authorize(Roles = "Student")]
	public class ProfileController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public ProfileController(IUnitofwork db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			ViewData["Title"] = "Your Profile";
			ViewData["userName"] = userName;
			ViewData["userEmail"] = userEmail;

			Models.Student? student = _db.Student.Include(u => u.Branch).FirstOrDefault(u => u.UserId == userId);
			return View(student);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateProfile()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			ViewData["Title"] = "Update Your Profile";
			ViewData["userName"] = userName;
			ViewData["userEmail"] = userEmail;

			Models.Student student = _db.Student.Get(u => u.UserId == userId);

			return View(student);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProfile(Models.Student student)
		{
			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(student.ImageFile.FileName);
			string filePath = Path.Combine(@"wwwroot\storage\student\images", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await student.ImageFile.CopyToAsync(stream);
			}

			student.ImageUrl = fileName;
			_db.Student.Update(student);
			_db.Save();
			TempData["success"] = "Profile details updated";
			return RedirectToAction("Index", "Profile", new { area = "Student" });
		}

		[HttpGet]
		public async Task<IActionResult> Other()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			ViewData["Title"] = "Other Profile Details";
			ViewData["userName"] = userName;
			ViewData["userEmail"] = userEmail;

			Models.Student student = _db.Student.Get(u => u.UserId == userId);

			return View(student);
		}

		[HttpPost]
		public IActionResult Other(Models.Student student)
		{
			_db.Student.Update(student);
			_db.Save();
			TempData["success"] = "Other details updated";
			return RedirectToAction("Index", "Profile", new { area = "Student" });
		}
	}
}
