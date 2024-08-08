using AcademicPerformance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ExamController : Controller
	{
		private readonly ApplicationDbContext _db;

		public ExamController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Exam> exams = _db.Exams.OrderByDescending(u => u.Year).ThenByDescending(u => u.Id).ToList();
			ViewData["Title"] = "Examinations";
			return View(exams);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewData["Title"] = "New Exam";
			return View();
		}

		[HttpPost]
		public IActionResult Create(Exam model)
		{
			Exam exam = _db.Exams.Where(u => u.Year == model.Year).Where(u => u.Month == model.Month).FirstOrDefault();
			if(exam != null)
			{
				TempData["error"] = "This exam is already created.";
				return RedirectToAction("Index", "Exam", new { area = "Admin" });
			}

			_db.Exams.Add(model);
			_db.SaveChanges();
			TempData["success"] = "Exam data created. Now teachers can add subjects amrks for students int this exam.";
			return RedirectToAction("Index", "Exam", new { area = "Admin" });
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			Exam exam = _db.Exams.Where(u => u.Id == id).FirstOrDefault();
			ViewData["Title"] = "Edit Exam " + exam.Year + " - " + exam.Month;
			return View(exam);
		}

		[HttpPost]
		public IActionResult Edit(Exam model)
		{
			_db.Exams.Update(model);
			_db.SaveChanges();
			TempData["success"] = "Exam data updated.";
			return RedirectToAction("Index", "Exam", new { area = "Admin" });
		}
	}
}
