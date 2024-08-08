using AcademicPerformance.Models;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademicPerformance.Areas.TPO.Controllers
{
	[Area("TPO")]
	[Authorize(Roles = "TPO")]
	public class JobsController : Controller
	{
		private readonly ApplicationDbContext _db;
		public JobsController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "Jobs";
			List<Jobs> jobs = _db.Jobs.Include(u => u.Branch).ToList();
			return View(jobs);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewData["Title"] = "New Job";
			IEnumerable<SelectListItem> branches = _db.Branches.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});
			JobsBranchVM subjectBranchVM = new JobsBranchVM()
			{
				Branches = branches,
				Jobs = new Jobs(),
			};
			return View(subjectBranchVM);
		}

		[HttpPost]
		public IActionResult Create(Jobs jobs)
		{
			_db.Jobs.Add(jobs);
			_db.SaveChanges();
			TempData["success"] = "Jobs created";
			return RedirectToAction("Index", "Jobs", new { area = "TPO" });
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			Jobs job = _db.Jobs.Where(u => u.Id == id).First();
			ViewData["Title"] = "Edit Job: " + job.Title;
			IEnumerable<SelectListItem> branches = _db.Branches.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});
			JobsBranchVM subjectBranchVM = new JobsBranchVM()
			{
				Branches = branches,
				Jobs = job,
			};
			return View(subjectBranchVM);
		}

		[HttpPost]
		public IActionResult Edit(Jobs jobs)
		{
			_db.Jobs.Update(jobs);
			_db.SaveChanges();
			TempData["success"] = "Jobs updated";
			return RedirectToAction("Index", "Jobs", new { area = "TPO" });
		}

		[HttpGet]
		public IActionResult Applications(int id)
		{
			List<JobsApply> applications = _db.JobsApplies.Where(u => u.JobsId == id).Include(u => u.Student).Include(u => u.Student.User).Include(u => u.Student.Branch).ToList();

			ViewData["Title"] = "Application for the job";
			return View(applications);
		}
	}
}
