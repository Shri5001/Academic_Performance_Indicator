using AcademicPerformance.Models;
using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SubjectController : Controller
	{
		private readonly IUnitofwork _db;
		public SubjectController(IUnitofwork db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			ViewData["Title"] = "Subjects";
			List<Subject> subjects = _db.Subject.Include(u => u.Branch).ToList();
			return View(subjects);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewData["Title"] = "New Subject";
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});
			SubjectBranchVM subjectBranchVM = new SubjectBranchVM()
			{
				Branches = branches,
				Subject = new Subject(),
			};
			return View(subjectBranchVM);
		}

		[HttpPost]
		public IActionResult Create(Subject subject)
		{
			_db.Subject.Add(subject);
			_db.Save();
			TempData["success"] = "Subject created";
			return RedirectToAction("Index", "Subject", new { area = "Admin" });
		}

		public IActionResult Edit(int id)
		{
			Subject subject = _db.Subject.Get(u => u.Id == id);
			ViewData["Title"] = "Edit Subject " + subject.Name;
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});
			SubjectBranchVM subjectBranchVM = new SubjectBranchVM()
			{
				Branches = branches,
				Subject = subject,
			};
			return View(subjectBranchVM);
		}

		[HttpPost]
		public IActionResult Edit(Subject subject)
		{
			_db.Subject.Update(subject);
			_db.Save();
			TempData["success"] = "Subject updated";
			return RedirectToAction("Index", "Subject", new { area = "Admin" });
		}
	}
}
