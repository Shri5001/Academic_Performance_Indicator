using AcademicPerformance.Models.Repository.IRepository;
using AcademicPerformance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcademicPerformance.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class StudentController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IWebHostEnvironment webHostEnvironment;
		public StudentController(IUnitofwork db, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
		{
			_db = db;
			_userManager = userManager;
			webHostEnvironment = hostEnvironment;
		}

		private string UploadedFile(StudentBarnchVM model)
		{
			string uniqueFileName = null;

			if (model.Image != null)
			{
				string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.Image.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

		[HttpGet]
		public async Task<IActionResult> Create(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Add details for student - " + user.FullName;
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			Models.Student student = _db.Student.Get(u => u.UserId == user.Id);
			if (student != null)
			{
				return RedirectToAction("Edit", "Student", new { area = "Admin", id = user.Id });
			}

			StudentBarnchVM studentBranchVM = new StudentBarnchVM()
			{
				Branches = branches,
				Student = new Models.Student(),
				User = user,
			};
			return View(studentBranchVM);
		}

		[HttpPost]
		public IActionResult Create(Models.Student student)
		{
			_db.Student.Add(student);
			_db.Save();
			TempData["success"] = "Student details updated";
			return RedirectToAction("Index", "User", new { area = "Admin" });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			ViewData["Title"] = "Add details for student - " + user.FullName;
			IEnumerable<SelectListItem> branches = _db.Branch.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			Models.Student student = _db.Student.Get(u => u.UserId == user.Id);

			StudentBarnchVM studentBranchVM = new StudentBarnchVM()
			{
				Branches = branches,
				Student = student,
				User = user,
			};
			return View(studentBranchVM);
		}

		[HttpPost]
		public IActionResult Edit(Models.Student student)
		{
			_db.Student.Update(student);
			_db.Save();
			TempData["success"] = "Student details updated";
			return RedirectToAction("Index", "User", new { area = "Admin" });
		}
	}
}
