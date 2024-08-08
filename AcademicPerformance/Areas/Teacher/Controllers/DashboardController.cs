using AcademicPerformance.Models.Data;
using AcademicPerformance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademicPerformance.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize(Roles = "Teacher")]

	public class DashboardController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly ApplicationDbContext _dba;
		private readonly UserManager<ApplicationUser> _userManager;
		public DashboardController(IUnitofwork db, UserManager<ApplicationUser> userManager, ApplicationDbContext dba)
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

			Models.Teacher teacher = _dba.Teachers.Where(u => u.UserId == userId).First();

			string isHod = "0";
			if(teacher.isHOD == 1)
			{
				isHod = "1";
			}
			ViewData["isHod"] = isHod;

			List<Exam> model = _dba.Exams.OrderByDescending(u => u.Id).ToList();
			ViewData["Title"] = "Dashboard";

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> GenerateReport(int id)
		{
			Exam exams = _dba.Exams.Where(u => u.Id == id).FirstOrDefault();

			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string userId = applicationUser?.Id;
			string? userEmail = applicationUser?.Email;
			string? userName = applicationUser?.FullName;

			Models.Teacher teacher = _dba.Teachers.Where(u => u.UserId == userId).First();

			List<Exam> model = _dba.Exams.OrderByDescending(u => u.Id).ToList();
			ViewData["Title"] = "Generate Report";
			ViewData["branchId"] = teacher.BranchId;

			return View(exams);
		}

		[HttpPost]
		public async Task<IActionResult> GenerateReport(int examId, int branchId, int year, int semester)
		{
			//Exam exams = _dba.Exams.Where(u => u.Id == id).FirstOrDefault();

			//ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			//string userId = applicationUser?.Id;
			//string? userEmail = applicationUser?.Email;
			//string? userName = applicationUser?.FullName;

			//Models.Teacher teacher = _dba.Teachers.Where(u => u.UserId == userId).First();

			//List<Exam> model = _dba.Exams.OrderByDescending(u => u.Id).ToList();
			//ViewData["Title"] = "Generate Report";
			//ViewData["branchId"] = teacher.BranchId;

			return RedirectToAction("ViewReport", "Dashboard", new { area = "Teacher", examId = examId, branchId = branchId, year = year, semester = semester });
		}

		[HttpGet]
		public async Task<IActionResult> ViewReport(int examId, int branchId, int year, int semester)
		{
			var subjects = _dba.TheoryMarks.Include(u => u.Subject).Where(u => u.ExamId == examId && u.BranchId == branchId && u.Subject.BranchYear == year && u.Subject.Sem == semester).Select(tm => tm.Subject).Distinct().ToList();

			// Create a list to hold the data for each subject
			var subjectDataList = new List<SubjectData>();

			// Calculate the total marks for each student
			var studentTotalMarks = _dba.TheoryMarks.Include(tm => tm.Subject).Where(tm => tm.Subject.BranchId == branchId && tm.Subject.BranchYear == year && tm.Subject.Sem == semester).GroupBy(tm => tm.StudentId)
				.Select(group => new
				{
					StudentId = group.Key,
					TotalMarks = group.Sum(tm => tm.Total),
				});
			// Calculate the percentage score for each student
			var studentPercentage = studentTotalMarks.Select(s => new
			{
				StudentId = s.StudentId,
				Percentage = subjects.Count != 0 ? Math.Round((decimal)s.TotalMarks / (subjects.Count * 100) * 100, 2) : 0 // Assuming each subject has 100 marks
			});

			// Get the top 3 students based on percentage score
			var top3Students = studentPercentage.OrderByDescending(s => s.Percentage)
												.Take(3)
												.ToList();

			// Map student details to a model for display
			var top3StudentsList = new List<TopStudentModel>();
			foreach (var student in top3Students)
			{
				var studentInfo = _dba.Students.Include(s => s.User)
											   .FirstOrDefault(s => s.Id == student.StudentId);

				top3StudentsList.Add(new TopStudentModel
				{
					StudentName = studentInfo.User.FullName,
					Percentage = student.Percentage
				});
			}

			foreach (var subject in subjects)
			{
				Models.TeacherSubject? teacher = _dba.TeacherSubjects.Where(u => u.SubjectId == subject.Id).Include(u => u.User).FirstOrDefault();
				
				var subjectData = new SubjectData
				{
					SubjectName = subject.Name,
					SubjectCode = subject.Code,
					PresentStudents = _dba.TheoryMarks.Count(tm => tm.SubjectId == subject.Id),
					PassedStudents = _dba.TheoryMarks.Count(tm => tm.SubjectId == subject.Id && tm.Status == 1),
					FailedStudents = _dba.TheoryMarks.Count(tm => tm.SubjectId == subject.Id && tm.Status == 0),
					Teacher = teacher.User.FullName,// Assuming Teacher is a property of Subject
				};

				// Calculate Passing Percentage
				if (subjectData.PresentStudents > 0)
				{
					subjectData.PassingPercentage = (decimal)subjectData.PassedStudents / subjectData.PresentStudents * 100;
				}

				subjectDataList.Add(subjectData);
			}

			// Calculate the total number of students appeared in the examination
			var countPresent = _dba.TheoryMarks.Select(tm => tm.StudentId).Distinct().Count();

			// Calculate the total number of students who passed all subjects
			var countPassed = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
											 .Count(group => group.All(tm => tm.Status == 1));

			// Calculate the total number of students who passed with ATKT
			var countAtkt = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
										   .Count(group => group.Any(tm => tm.Status == 1 && tm.Status != 0));

			// Calculate the total number of students who failed
			var countFailed = countPresent - countPassed;

			// Calculate the total number of students who passed with distinction
			var countDistinct = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
											   .Count(group => group.Average(tm => tm.Total) >= 75);

			// Calculate the total number of students who passed with first class
			var countFirst = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
											.Count(group => group.Average(tm => tm.Total) >= 60 && group.Average(tm => tm.Total) < 75);

			// Calculate the total number of students who passed with higher second class
			var countSecondHigh = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
												 .Count(group => group.Average(tm => tm.Total) >= 55 && group.Average(tm => tm.Total) < 60);

			// Calculate the total number of students who passed with second class
			var countSecond = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
											 .Count(group => group.Average(tm => tm.Total) >= 50 && group.Average(tm => tm.Total) < 55);

			// Calculate the total number of students who passed with pass class
			var countPassing = _dba.TheoryMarks.GroupBy(tm => tm.StudentId)
											  .Count(group => group.Average(tm => tm.Total) >= 40 && group.Average(tm => tm.Total) < 50);

			// Store the calculated statistics in ViewData
			ViewData["countPresent"] = countPresent;
			ViewData["countPassed"] = countPassed;
			ViewData["countAtkt"] = countAtkt;
			ViewData["countFailed"] = countFailed;
			ViewData["countDistinct"] = countDistinct;
			ViewData["countFirst"] = countFirst;
			ViewData["countSecondHigh"] = countSecondHigh;
			ViewData["countSecond"] = countSecond;
			ViewData["countPassing"] = countPassing;

			// Pass subjectDataList to the view
			ViewData["Title"] = "Report";
			return View(new ReportViewModel
			{
				SubjectDataList = subjectDataList,
				TopStudents = top3StudentsList
			});
		}
	}
}
