using AcademicPerformance.Models;
using AcademicPerformance.Models.Repository;
using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class TeacherController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly ApplicationDbContext _dba;
		private readonly UserManager<ApplicationUser> _userManager;
		public TeacherController(IUnitofwork db, UserManager<ApplicationUser> userManager, ApplicationDbContext dba)
		{
			_db = db;
			_userManager = userManager;
			_dba = dba;
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "Teachers";
			List<Models.Teacher> teachers = _db.Teacher.Include(u => u.Branch).ToList();
			return View(teachers);
		}

		[HttpGet]
		public async Task<IActionResult> Create(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Add details for teacher - " + user.FullName;
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			Models.Teacher teacher = _db.Teacher.Get(u => u.UserId == user.Id);
			if(teacher != null)
			{
				return RedirectToAction("Edit", "Teacher", new { area = "Admin", id = user.Id });
			}

			TeacherBranchVM teacherBranchVM = new TeacherBranchVM()
			{
				Branches = branches,
				Teacher = new Models.Teacher(),
				User = user,
			};
			return View(teacherBranchVM);
		}

		[HttpPost]
		public IActionResult Create(Models.Teacher teacher)
		{
			_db.Teacher.Add(teacher);
			_db.Save();
			TempData["success"] = "Teacher details updated";
			return RedirectToAction("Index", "User", new { area = "Admin" });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Add details for teacher - " + user.FullName;
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			Models.Teacher teacher = _db.Teacher.Get(u => u.UserId == user.Id);

			TeacherBranchVM teacherBranchVM = new TeacherBranchVM()
			{
				Branches = branches,
				Teacher = teacher,
				User = user,
			};
			return View(teacherBranchVM);
		}

		[HttpPost]
		public IActionResult Edit(Models.Teacher teacher)
		{
			_db.Teacher.Update(teacher);
			_db.Save();
			TempData["success"] = "Teacher details updated";
			return RedirectToAction("Index", "User", new { area = "Admin" });
		}

		// Subject Part
		public async Task<IActionResult> Subjects(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Subjects for teacher - " + user.FullName;
			ViewData["userId"] = id;

			IEnumerable<SelectListItem> subjects = _db.Subject.GetAll().Select(u => new SelectListItem
			{
				Text = u.Code + " - " + u.Name,
				Value = u.Id.ToString(),
			});

			List<TeacherSubject>? teacher = _dba.TeacherSubjects.Where(u => u.UserId == id).Include(s => s.Subject).ToList();
			return View(teacher);
		}

		[HttpGet]
		public async Task<IActionResult> AddSubject(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Add subjects for teacher - " + user.FullName;

			IEnumerable<SelectListItem> subjects = _db.Subject.GetAll().Select(u => new SelectListItem
			{
				Text = u.Code + " - " + u.Name,
				Value = u.Id.ToString(),
			});

			TeacherSubjectVM model = new TeacherSubjectVM()
			{
				Subjects = subjects,
				Teacher = new TeacherSubject(),
				User = user,
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult AddSubject(TeacherSubject teacher)
		{
			_dba.TeacherSubjects.Add(teacher);
			_dba.SaveChanges();
			TempData["success"] = "Teacher subject details updated";
			return RedirectToAction("Subjects", "Teacher", new { area = "Admin", id = teacher.UserId });
		}

		public async Task<IActionResult> EditSubject(int id)
		{
			ViewData["Title"] = "Edit subjects";

			IEnumerable<SelectListItem> subjects = _db.Subject.GetAll().Select(u => new SelectListItem
			{
				Text = u.Code + " - " + u.Name,
				Value = u.Id.ToString(),
			});

			TeacherSubject? teacher = _dba.TeacherSubjects.Where(u => u.Id == id).FirstOrDefault();
			var user = await _userManager.FindByIdAsync(teacher.UserId);

			TeacherSubjectVM model = new TeacherSubjectVM()
			{
				Subjects = subjects,
				Teacher = teacher,
				User = user,
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult EditSubject(TeacherSubject teacher)
		{
			_dba.TeacherSubjects.Update(teacher);
			_dba.SaveChanges();
			TempData["success"] = "Teacher subject details updated";
			return RedirectToAction("Subjects", "Teacher", new { area = "Admin", id = teacher.UserId });
		}
	}
}
