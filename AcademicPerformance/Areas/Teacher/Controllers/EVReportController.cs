using AcademicPerformance.Models;
using AcademicPerformance.Models.Data;
using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AcademicPerformance.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize(Roles = "Teacher")]
	public class EVReportController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly ApplicationDbContext _dba;
		private readonly UserManager<ApplicationUser> _userManager;
		public EVReportController(IUnitofwork db, UserManager<ApplicationUser> userManager, ApplicationDbContext dba)
		{
			_db = db;
			_userManager = userManager;
			_dba = dba;
		}

		public async Task<IActionResult> Index()
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			List<EvaluationReport> evReports = _dba.EVReports.Where(u => u.UserId == userId).Include(b => b.Branch).Include(s => s.Subject).ToList();
			ViewData["Title"] = "Continous Evaluation Reports";
			return View(evReports);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewData["Title"] = "New Continous Evaluation Report";

			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			IEnumerable<SelectListItem> subjects = _db.Subject.GetAll().Select(u => new SelectListItem
			{
				Text = u.Code + "-" + u.Name,
				Value = u.Id.ToString(),
			});

			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string? userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			EVReportBranchVM evBranchVM = new EVReportBranchVM()
			{
				Branches = branches,
				EVReport = new EvaluationReport(),
				User = applicationUser,
				Subjects = subjects,
			};
			return View(evBranchVM);
		}

		[HttpPost]
		public IActionResult Create(EVReportBranchVM evdata)
		{
			_db.EVReport.Add(evdata.EVReport);
			_db.Save();
			TempData["success"] = "Inital data for report added";
			return RedirectToAction("Index", "EVReport", new { area = "Teacher" });
		}

		[HttpGet]
		public IActionResult FillMarks(int id)
		{
			EvaluationReport evReport = _db.EVReport.Get(u => u.Id == id);
			Branch branch = _db.Branch.Get(u => u.Id == evReport.BranchId);
			Subject subject = _db.Subject.Get(u => u.Id == evReport.SubjectId);
			List<Models.Student> students = _db.Student.IncludeWhere(u => u.User, s => s.BranchId == evReport.BranchId && s.ClassYear == evReport.BranchYear && s.Sem == evReport.Sem).ToList();

			EvaluationViewModel viewModel = new EvaluationViewModel
			{
				EvaluationReport = evReport,
				Students = students
			};

			ViewData["Title"] = "Fill Marks";
			ViewData["SubTitle"] = "Academic Year: " + evReport.Year + ". " + branch.Name + " - Year: " + evReport.BranchYear.ToString() + " SEM: " + evReport.Sem.ToString() + ". Subject: " + subject.Code + "-" + subject.Name;

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult FillMarks(EvaluationViewModel model)
		{
			var studentData = JsonConvert.DeserializeObject<List<StudentData>>(model.EvaluationReport.ReportData);
			model.EvaluationReport.ReportData = JsonConvert.SerializeObject(studentData);
			_db.EVReport.Update(model.EvaluationReport);
			_db.Save();
			TempData["success"] = "Data for report added";
			return RedirectToAction("Index", "EVReport", new { area = "Teacher" });
		}

		[HttpGet]
		public IActionResult ShowReport(int id)
		{
			EvaluationReport evReport = _db.EVReport.Get(u => u.Id == id);
			Branch branch = _db.Branch.Get(u => u.Id == evReport.BranchId);
			Subject subject = _db.Subject.Get(u => u.Id == evReport.SubjectId);

			var studentDataList = JsonConvert.DeserializeObject<List<StudentData>>(evReport.ReportData);

			ViewData["Title"] = "Report Data";
			ViewData["SubTitle"] = "Academic Year: " + evReport.Year + ". " + branch.Name + " - Year: " + evReport.BranchYear.ToString() + " SEM: " + evReport.Sem.ToString() + ". Subject: " + subject.Code + "-" + subject.Name;

			return View(studentDataList);
		}
	}
}
