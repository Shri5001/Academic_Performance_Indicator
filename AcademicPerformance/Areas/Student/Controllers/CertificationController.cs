using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicPerformance.Areas.Student.Controllers
{
	[Area("Student")]
	[Authorize(Roles = "Student")]
	public class CertificationController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public CertificationController(IUnitofwork db, UserManager<ApplicationUser> userManager)
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

			ViewData["Title"] = "Your Certifications";
			ViewData["userName"] = userName;
			ViewData["userEmail"] = userEmail;

			List<StudentCertification> certifications = _db.StudentCertification.GetWhere(u => u.UserId == userId).ToList();
			return View(certifications);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			ViewData["Title"] = "Add Certificate";
			ViewData["userId"] = userId;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(StudentCertification model)
		{
			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocumentFile.FileName);
			string filePath = Path.Combine(@"wwwroot\storage\student\certificate", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await model.DocumentFile.CopyToAsync(stream);
			}

			model.FileUrl = fileName;

			_db.StudentCertification.Add(model);
			_db.Save();
			TempData["success"] = "Certification Added";
			return RedirectToAction("Index", "Certification", new { area = "Student" });
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			StudentCertification model = _db.StudentCertification.Get(u => u.Id == id);
			ViewData["Title"] = "Delete Certification?";
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(StudentCertification model)
		{
			_db.StudentCertification.Remove(model);
			_db.Save();
			TempData["success"] = "Certification details removed";
			return RedirectToAction("Index", "Education", new { area = "Student" });
		}
	}
}
