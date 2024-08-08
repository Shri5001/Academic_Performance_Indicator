using AcademicPerformance.Models;
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
	public class ExamController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public ExamController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			List<Exam> exams = _db.Exams.OrderByDescending(u => u.Id).ToList();
			ViewData["Title"] = "Examinations";
			return View(exams);
		}

		public async Task<IActionResult> Subjects(int id)
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string userId = applicationUser?.Id;

			Exam exams = _db.Exams.Where(u => u.Id == id).FirstOrDefault();

			List<ExamSubject> subjects = _db.ExamSubjects.Include(u => u.Subject).Where(u => u.UserId == userId).Where(u => u.ExamId == id).OrderBy(u => u.Year).ThenBy(u => u.Sem).ToList();
			ViewData["Title"] = "Subjects in exam of " + exams.Month + " " + exams.Year;
			ViewData["examId"] = exams.Id;
			return View(subjects);
		}

		[HttpGet]
		public async Task<IActionResult> CreateSubjects(int id)
		{
			ApplicationUser? applicationUser = await _userManager.GetUserAsync(User);
			string userId = applicationUser?.Id;

			Models.Teacher teacher = _db.Teachers.Where(u => u.UserId == userId).FirstOrDefault();

			Exam exams = _db.Exams.Where(u => u.Id == id).FirstOrDefault();

			IEnumerable<SelectListItem> subjects = _db.TeacherSubjects.Include(u => u.User).Include(s => s.Subject).Where(u => u.UserId == userId).Select(u => new SelectListItem
			{
				Text = u.Subject.Code + "-" +u.Subject.Name,
				Value = u.SubjectId.ToString(),
			});

			ViewData["Title"] = "Select subject for exam";

			TeacherExamSubjectVM model = new TeacherExamSubjectVM()
			{
				Subjects = subjects,
				Exam = exams,
				ExamSubject = new ExamSubject(),
				User = applicationUser,
				BranchId = teacher.BranchId,
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult CreateSubjects(TeacherExamSubjectVM model)
		{
			ExamSubject examSubject = _db.ExamSubjects.Where(u => u.ExamId == model.ExamSubject.ExamId).Where(u => u.SubjectId == model.ExamSubject.SubjectId).FirstOrDefault();

			if(examSubject != null)
			{
				TempData["error"] = "Subject already present in this exam";
				return RedirectToAction("Subjects", "Exam", new { area = "Teacher", id = model.ExamSubject.ExamId });
			}

			_db.ExamSubjects.Add(model.ExamSubject);
			_db.SaveChanges();

			TempData["success"] = "Subject added to exam";
			return RedirectToAction("Subjects", "Exam", new { area = "Teacher", id = model.ExamSubject.ExamId });
		}

		[HttpGet]
		public IActionResult AddTheoryMarks(int examId, int subjectId, int branchId, int examSubjectId)
		{
			Exam exam = _db.Exams.Where(u => u.Id == examId).FirstOrDefault();
			ExamSubject examSubject = _db.ExamSubjects.Where(u => u.Id == examSubjectId).FirstOrDefault();
			Subject subject = _db.Subjects.Where(u => u.Id == subjectId).FirstOrDefault();
			Branch branch = _db.Branches.Where(u => u.Id == branchId).FirstOrDefault();
			IEnumerable<SelectListItem> students = _db.Students.Include(u => u.User)
				.Where(u => u.BranchId == branchId)
				.Where(u => u.ClassYear == examSubject.Year)
				.Where(u => u.Sem == examSubject.Sem)
				.ToList()
				.Select(u => new SelectListItem
				{
					Text = u.User.FullName,
					Value = u.Id.ToString(),
				});

			TheoryExamStudentVM model = new TheoryExamStudentVM()
			{
				Exam = exam,
				ExamSubject = examSubject,
				Subject = subject,
				Branch = branch,
				Students = students,
				TheoryMarks = new TheoryMarks(),
				TheoryMarksList = _db.TheoryMarks.Include(u => u.Exam).Include(u => u.Subject)
				.Where(u => u.ExamId == examId)
				.Where(u => u.Subject.Id == subjectId)
				.Where(u => u.Subject.BranchId == branchId).ToList(),
			};
			ViewData["Title"] = "Enter Marks for students";
			return View(model);
		}

		[HttpPost]
		public IActionResult AddTheoryMarks(TheoryExamStudentVM model)
		{
			TheoryMarks theoryResult = _db.TheoryMarks.Where(u => u.ExamId == model.TheoryMarks.ExamId).Where(u => u.SubjectId == model.TheoryMarks.SubjectId).Where(u => u.StudentId == model.TheoryMarks.StudentId).FirstOrDefault();

			int examId = model.TheoryMarks.ExamId;
			int subjectId = model.TheoryMarks.SubjectId;
			int branchId = model.TheoryMarks.BranchId;
			int examSubjectId = model.ExamSubject.Id;

			if (theoryResult == null)
			{
				model.TheoryMarks.Total = model.TheoryMarks.InSem + model.TheoryMarks.EndSem;

				int Status = 1;
				if(model.TheoryMarks.Total < 40 || model.TheoryMarks.EndSem < 28 || model.TheoryMarks.Attendance == "AB")
				{
					Status = 0;
				}

				model.TheoryMarks.Status = Status;

				_db.TheoryMarks.Add(model.TheoryMarks);
				_db.SaveChanges();
				TempData["success"] = "Marks added.";
			}

			return RedirectToAction("AddTheoryMarks", "Exam", new { area = "Teacher", examId = model.TheoryMarks.ExamId, subjectId = model.TheoryMarks.SubjectId, branchId = model.TheoryMarks.BranchId, examSubjectId = model.ExamSubject.Id });
		}


		// Practical Marks
		[HttpGet]
		public IActionResult AddPracticalMarks(int examId, int subjectId, int branchId, int examSubjectId)
		{
			Exam exam = _db.Exams.Where(u => u.Id == examId).FirstOrDefault();
			ExamSubject examSubject = _db.ExamSubjects.Where(u => u.Id == examSubjectId).FirstOrDefault();
			Subject subject = _db.Subjects.Where(u => u.Id == subjectId).FirstOrDefault();
			Branch branch = _db.Branches.Where(u => u.Id == branchId).FirstOrDefault();
			IEnumerable<SelectListItem> students = _db.Students.Include(u => u.User)
				.Where(u => u.BranchId == branchId)
				.Where(u => u.ClassYear == examSubject.Year)
				.Where(u => u.Sem == examSubject.Sem)
				.ToList()
				.Select(u => new SelectListItem
				{
					Text = u.User.FullName,
					Value = u.Id.ToString(),
				});
			List<ExperimentMarksVM> experimentMarks = new List<ExperimentMarksVM>();
			for (int i = 1; i <= 12; i++)
			{
				List<SubsectionMarksVM> subsectionMarks = new List<SubsectionMarksVM>();
				for (int j = 1; j <= 4; j++)
				{
					subsectionMarks.Add(new SubsectionMarksVM
					{
						SubsectionName = $"Subsection {j}",
						Marks = 0 // Set default value for marks
					});
				}

				experimentMarks.Add(new ExperimentMarksVM
				{
					ExperimentName = $"Experiment {i}",
					SubsectionMarks = subsectionMarks
				});
			}

			PracticalExamStudentVM model = new PracticalExamStudentVM()
			{
				Exam = exam,
				ExamSubject = examSubject,
				Subject = subject,
				Branch = branch,
				Students = students,
				PracticalMarks = new PracticalMarks(),
				ExperimentMarks = experimentMarks,
				PraticalMarksList = _db.PracticalMarks.Include(u => u.Exam).Include(u => u.Subject)
				.Where(u => u .ExamId == examId)
				.Where(u => u.Subject.Id == subjectId)
				.Where(u => u.Subject.BranchId == branchId).ToList(),
			};
			ViewData["Title"] = "Enter Marks for students";
			return View(model);
		}

		[HttpPost]
		public IActionResult AddPracticalMarks(PracticalExamStudentVM model)
		{
			PracticalMarks practicalMarks = _db.PracticalMarks.FirstOrDefault(u => u.ExamId == model.PracticalMarks.ExamId &&
																		u.SubjectId == model.PracticalMarks.SubjectId &&
																		u.StudentId == model.PracticalMarks.StudentId);

			if (practicalMarks == null)
			{
				// Deserialize the ExperimentMarks JSON data
				var experiments = JsonConvert.DeserializeObject<List<ExperimentMarksVM>>(model.PracticalMarks.ExperimentMarks);

				// Calculate the total marks from all experiments
				int totalMarksFromExperiments = experiments.Sum(exp => exp.SubsectionMarks.Sum(sub => sub.Marks));

				// Update the model with the calculated total marks
				model.PracticalMarks.Total480 = totalMarksFromExperiments;

				// Calculate the equivalent TermWork marks (25 marks)
				float equivalentTermWorkMarks = totalMarksFromExperiments * 25 / 480;

				// Update the model with the equivalent TermWork marks
				model.PracticalMarks.TermWork = equivalentTermWorkMarks;

				// Calculate the total marks including external practical
				model.PracticalMarks.Total = model.PracticalMarks.TermWork + model.PracticalMarks.ExternalPractical;

				// Determine the status based on the conditions provided
				int status = 1;
				if (model.PracticalMarks.ExternalPractical < 11 || model.PracticalMarks.TermWork < 11 || model.PracticalMarks.Attendance == "AB")
				{
					status = 0;
				}

				// Update the model with the calculated status
				model.PracticalMarks.Status = status;

				// Add the practical marks to the database context and save changes
				_db.PracticalMarks.Add(model.PracticalMarks);
				_db.SaveChanges();

				TempData["success"] = "Marks added.";
			}

			return RedirectToAction("AddPracticalMarks", "Exam", new { area = "Teacher", examId = model.PracticalMarks.ExamId, subjectId = model.PracticalMarks.SubjectId, branchId = model.PracticalMarks.BranchId, examSubjectId = model.ExamSubject.Id });
		}

	}
}
