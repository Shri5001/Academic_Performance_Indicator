using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicPerformance.Areas.Student.Controllers
{
	[Area("Student")]
	[Authorize(Roles = "Student")]
	public class EducationController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public EducationController(IUnitofwork db, UserManager<ApplicationUser> userManager)
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

			ViewData["Title"] = "Your Education";
			ViewData["userName"] = userName;
			ViewData["userEmail"] = userEmail;

			List<StudentEducation> certifications = _db.StudentEducation.GetWhere(u => u.UserId == userId).ToList();
			return View(certifications);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			ViewData["Title"] = "Add Eductaion";
			ViewData["userId"] = userId;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(StudentEducation model)
		{
			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocumentFile.FileName);
			string filePath = Path.Combine(@"wwwroot\storage\student\education", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await model.DocumentFile.CopyToAsync(stream);
			}

			model.FileUrl = fileName;

			_db.StudentEducation.Add(model);
			_db.Save();
			TempData["success"] = "Education details Added";
			return RedirectToAction("Index", "Education", new { area = "Student" });
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			StudentEducation model =  _db.StudentEducation.Get(u => u.Id == id);
			ViewData["Title"] = "Delete Education?";
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(StudentEducation model)
		{
			_db.StudentEducation.Remove(model);
			_db.Save();
			TempData["success"] = "Education details removed";
			return RedirectToAction("Index", "Education", new { area = "Student" });
		}
	}
}
