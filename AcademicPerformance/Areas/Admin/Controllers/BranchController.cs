using AcademicPerformance.Models;
using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicPerformance.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class BranchController : Controller
	{
		private readonly IUnitofwork _db;
		public BranchController(IUnitofwork db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "Branches";
			List<Branch> branches = _db.Branch.GetAll().ToList();
			return View(branches);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewData["Title"] = "New Branch";
			return View();
		}

		[HttpPost]
		public IActionResult Create(Branch branch)
		{
			_db.Branch.Add(branch);
			_db.Save();
			TempData["success"] = "Branch created";
			return RedirectToAction("Index", "Branch", new { area = "Admin" });
		}

		public IActionResult Edit(int id)
		{
			Branch branch = _db.Branch.Get(u => u.Id == id);
			ViewData["Title"] = "Edit Branch " + branch.Name;
			return View(branch);
		}

		[HttpPost]
		public IActionResult Edit(Branch branch)
		{
			_db.Branch.Update(branch);
			_db.Save();
			TempData["success"] = "Branch updated";
			return RedirectToAction("Index", "Branch", new { area = "Admin" });
		}
	}
}
