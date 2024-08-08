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
	public class TheoryResultController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly ApplicationDbContext _dba;
		private readonly UserManager<ApplicationUser> _userManager;
		public TheoryResultController(IUnitofwork db, UserManager<ApplicationUser> userManager, ApplicationDbContext dba)
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

			List<TheoryResult> model = _dba.TheoryResults.Where(u => u.UserId == userId).Include(b => b.Branch).Include(s => s.Subject).ToList();
			ViewData["Title"] = "Theory Result Reports";

			//IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			//{
			//	Text = u.Name,
			//	Value = u.Id.ToString(),
			//});
			//ViewData["branches"] = branches;

			//IEnumerable<SelectListItem> subjects = _db.Subject.GetAll().Select(u => new SelectListItem
			//{
			//	Text = u.Code + " - " + u.Name,
			//	Value = u.Id.ToString(),
			//});
			//ViewData["subjects"] = subjects;

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewData["Title"] = "New Theory Result Report";

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

			TheoryBranchVM model = new TheoryBranchVM()
			{
				Branches = branches,
				TheoryResult = new TheoryResult(),
				User = applicationUser,
				Subjects = subjects,
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult Create(TheoryBranchVM model)
		{
			_dba.TheoryResults.Add(model.TheoryResult);
			_db.Save();
			TempData["success"] = "Inital data for report added";
			return RedirectToAction("Index", "TheoryResult", new { area = "Teacher" });
		}

		[HttpGet]
		public IActionResult FillMarks(int id)
		{
			TheoryResult evReport = _dba.TheoryResults.Where(u => u.Id == id).FirstOrDefault();
			Branch branch = _db.Branch.Get(u => u.Id == evReport.BranchId);
			Subject subject = _db.Subject.Get(u => u.Id == evReport.SubjectId);
			List<Models.Student> students = _db.Student.IncludeWhere(u => u.User, s => s.BranchId == evReport.BranchId && s.ClassYear == evReport.BranchYear && s.Sem == evReport.Sem).ToList();

			TheoryResultViewModel viewModel = new TheoryResultViewModel
			{
				TheoryResult = evReport,
				Students = students
			};

			ViewData["Title"] = "Fill Marks";
			ViewData["SubTitle"] = "Academic Year: " + evReport.Year + ". " + branch.Name + " - Year: " + evReport.BranchYear.ToString() + " SEM: " + evReport.Sem.ToString() + ". Subject: " + subject.Code + "-" + subject.Name;

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult FillMarks(TheoryResultViewModel model)
		{
			var studentData = JsonConvert.DeserializeObject<List<TheoryData>>(model.TheoryResult.ReportData);
			model.TheoryResult.ReportData = JsonConvert.SerializeObject(studentData);
			_dba.TheoryResults.Update(model.TheoryResult);
			_db.Save();
			TempData["success"] = "Data for report added";
			return RedirectToAction("Index", "TheoryResult", new { area = "Teacher" });
		}

		[HttpGet]
		public IActionResult ShowReport(int id)
		{
			TheoryResult evReport = _dba.TheoryResults.Where(u => u.Id == id).FirstOrDefault();
			Branch branch = _db.Branch.Get(u => u.Id == evReport.BranchId);
			Subject subject = _db.Subject.Get(u => u.Id == evReport.SubjectId);

			var studentDataList = JsonConvert.DeserializeObject<List<TheoryData>>(evReport.ReportData);

			ViewData["Title"] = "Report Data";
			ViewData["SubTitle"] = "Academic Year: " + evReport.Year + ". " + branch.Name + " - Year: " + evReport.BranchYear.ToString() + " SEM: " + evReport.Sem.ToString() + ". Subject: " + subject.Code + "-" + subject.Name;

			return View(studentDataList);
		}

		[HttpGet]
		public IActionResult GenerateReport()
		{
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			GenerateReportGetVM viewModel = new GenerateReportGetVM
			{
				Branches = branches,
			};

			ViewData["Title"] = "Generate Report";
			return View(viewModel);
		}

		[HttpPost]
		public IActionResult GenerateReport(GenerateReportGetVM model)
		{
			return RedirectToAction("GeneratedReport", "TheoryResult", new { area = "Teacher", branch=model.SelectedBranchId, year=model.Year, branchyear=model.BranchYear, sem=model.Sem });
		}

		[HttpGet]
		public async Task<IActionResult> GeneratedReport(int branch, string year, int branchyear, int sem)
		{
			List<TheoryResult> result = _dba.TheoryResults.Where(b => b.BranchId == branch)
				.Where(y => y.Year == year)
				.Where(by => by.BranchYear == branchyear)
				.Where(s => s.Sem == sem)
				.Include(brnch => brnch.Branch)
				.Include(sub => sub.Subject)
				.ToList();

			List<Subject> subjects = _dba.Subjects.Where(b => b.BranchId == branch).Where(by => by.BranchYear == branchyear)
				.Where(s => s.Sem == sem).ToList();

			List<TheoryData> theoryData = new List<TheoryData>();
			foreach (var data in result)
			{
				theoryData.AddRange(JsonConvert.DeserializeObject<List<TheoryData>>(data.ReportData));
			}

			GeneratedTheoryReportVM model = new GeneratedTheoryReportVM
			{
				Subjects = subjects,
				TheoryResults = result,
				TheoryDatas = theoryData,
			};

			List<SubjectStatistics> subjectStatistics = await CalculateSubjectStatistics(subjects, result);

			// Calculate counts
			int countPresent = theoryData.Select(data => data.SeatNumber).Distinct().Count();
			int countFailed = theoryData.Count(data => data.Marks == 0); // Students with zero marks
			int countPassed = theoryData.Count(data => data.Marks > 0); // Students with non-zero marks
			int countAtkt = subjects.Count(sub => result.Count(r => r.SubjectId == sub.Id) >= 2); // Subjects with 2 or more failed
			int countDistinct = theoryData.Count(data => (float)data.Marks / data.Total * 100 >= 75);
			int countFirst = theoryData.Count(data => (float)data.Marks / data.Total * 100 >= 65 && (float)data.Marks / data.Total * 100 < 75);
			int countSecondHigh = theoryData.Count(data => (float)data.Marks / data.Total * 100 >= 55 && (float)data.Marks / data.Total * 100 < 65);
			int countSecond = theoryData.Count(data => (float)data.Marks / data.Total * 100 >= 50 && (float)data.Marks / data.Total * 100 < 55);
			int countPassing = theoryData.Count(data => (float)data.Marks / data.Total * 100 >= 40 && (float)data.Marks / data.Total * 100 < 50);

			// Prepare ViewData
			ViewData["Title"] = "Theory Exam Result Analysis";
			ViewData["countPresent"] = countPresent;
			ViewData["countFailed"] = countFailed;
			ViewData["countPassed"] = countPassed;
			ViewData["countAtkt"] = countAtkt;
			ViewData["countDistinct"] = countDistinct;
			ViewData["countFirst"] = countFirst;
			ViewData["countSecondHigh"] = countSecondHigh;
			ViewData["countSecond"] = countSecond;
			ViewData["countPassing"] = countPassing;

			return View(subjectStatistics);
		}

		public async Task<List<SubjectStatistics>> CalculateSubjectStatistics(List<Subject> subjects, List<TheoryResult> results)
		{
			List<SubjectStatistics> subjectStatistics = new List<SubjectStatistics>();

			foreach (var subject in subjects)
			{
				var subjectResult = results.Where(r => r.SubjectId == subject.Id).ToList();
				List<TheoryData> theoryDatas = subjectResult.SelectMany(r => JsonConvert.DeserializeObject<List<TheoryData>>(r.ReportData)).ToList();

				// Calculate statistics
				int totalStudents = theoryDatas.Count;
				int zeroMarksStudents = theoryDatas.Count(data => data.Marks == 0); // Students with zero marks
				int failedStudents = theoryDatas.Count(data => data.Marks > 0 && data.Marks < (data.Total * 0.4));
				int passedStudents = theoryDatas.Count(data => data.Marks >= (data.Total * 0.4)); // Assuming passing marks is 40%
				// int failedStudents = totalStudents - passedStudents; // Failed students include zero marks and those below passing threshold

				// Include zero marks students in failed count
				failedStudents += zeroMarksStudents;

				float passingPercentage;

				if (totalStudents > 0)
				{
					// Calculate passing percentage based on total students
					passingPercentage = (float)passedStudents / totalStudents * 100;
				}
				else
				{
					passingPercentage = 0;
				}

				TeacherSubject teacherSubject = _dba.TeacherSubjects.Where(u => u.SubjectId == subject.Id).FirstOrDefault();

				var user = await _userManager.FindByIdAsync(teacherSubject.UserId);

				subjectStatistics.Add(new SubjectStatistics
				{
					Subject = subject,
					PresentStudents = totalStudents - zeroMarksStudents, // Exclude zero marks students from total
					PassedStudents = passedStudents,
					FailedStudents = failedStudents,
					PassingPercentage = passingPercentage,
					Teacher = user,
				});
			}

			return subjectStatistics;
		}
	}
}
