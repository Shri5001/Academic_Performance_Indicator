using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcademicPerformance.Models.ServiceFilter;
using Microsoft.EntityFrameworkCore;
using AcademicPerformance.Models.VM;

namespace AcademicPerformance.Areas.Student.Controllers
{
	[Area("Student")]
	[Authorize(Roles = "Student")]
	public class DashboardController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}

		[ServiceFilter(typeof(CheckStudentProfileFilter))]
		public async Task<IActionResult> Index()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;

			Models.Student student = _db.Students.Where(u => u.UserId == userId).FirstOrDefault();

			List<StudentEducation> education = _db.StudentEducations.Where(u => u.UserId == userId).ToList();
			List<StudentCertification> certfication = _db.StudentCertifications.Where(u => u.UserId == userId).ToList();

			List<Jobs> jobs = _db.Jobs.Where(u => u.BranchId == student.BranchId).Include(u => u.Branch).ToList();

			StudentHomeVM model = new StudentHomeVM()
			{
				Jobs = jobs,
				Student = student,
				Education = education,
				Certification = certfication,
			};

			ViewData["Title"] = "Dashboard";
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ViewJob(int id)
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;

			Models.Student student = _db.Students.Where(u => u.UserId == userId).FirstOrDefault();
			Jobs job = _db.Jobs.Where(u => u.Id == id).First();
			StudentJobApplyVM model = new StudentJobApplyVM()
			{
				Job = job,
				JobsApply = new JobsApply(),
				Student = student,
			};

			ViewData["Title"] = job.Title;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ViewJob(StudentJobApplyVM model)
		{
			JobsApply apply = _db.JobsApplies.Where(u => u.StudentId == model.JobsApply.StudentId).Where(u => u.JobsId == model.JobsApply.JobsId).FirstOrDefault();
			
			if(apply == null)
			{
				_db.JobsApplies.Add(model.JobsApply);
				_db.SaveChanges();
				TempData["success"] = "Applied successfully";
			}

			return RedirectToAction("Index", "Dashboard", new { area = "Student" });
		}
	}
}
