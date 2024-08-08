using AcademicPerformance.Models;
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
	public class UserController : Controller
	{
		private readonly IUnitofwork _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public UserController(IUnitofwork db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "Users";
			var users = _userManager.Users
			.ToList()
			.Select(c => new UserWithRole()
			{
				UserId = c.Id,
				Username = c.UserName,
				Email = c.Email,
				Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray()),
				FullName = c.FullName
			})
			.ToList();
			return View(users);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewData["Title"] = "Create new user";
			var roles = await _roleManager.Roles.ToListAsync();
			var model = new RegisterVM
			{
				AvailableRoles = roles.Select(r => new SelectListItem
				{
					Text = r.Name,
					Value = r.Name
				})
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(RegisterVM model)
		{
			var roles = await _roleManager.Roles.ToListAsync();
			model.AvailableRoles = roles.Select(r => new SelectListItem
			{
				Text = r.Name,
				Value = r.Name
			});

			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					// Assign selected role upon registration
					if (!string.IsNullOrEmpty(model.SelectedRole))
					{
						await _userManager.AddToRoleAsync(user, model.SelectedRole);
					}

					// Redirect to login page after successful registration
					if(model.SelectedRole == "Teacher")
					{
						return RedirectToAction("Create", "Teacher", new { area = "Admin", id = user.Id });
					}
					if (model.SelectedRole == "Student")
					{
						return RedirectToAction("Create", "Student", new { area = "Admin", id = user.Id });
					}
					return RedirectToAction("Index", "User", new {area = "Admin"});
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			ViewData["Title"] = "Edit " + user.FullName;

			// Retrieve available roles
			var roles = await _roleManager.Roles.ToListAsync();

			// Get the user's current role
			var userRoles = await _userManager.GetRolesAsync(user);
			var currentRole = userRoles.FirstOrDefault();

			var model = new RegisterVM
			{
				Email = user.Email,
				FullName = user.FullName,
				SelectedRole = currentRole, // Set the user's current role as selected
				AvailableRoles = roles.Select(r => new SelectListItem
				{
					Text = r.Name,
					Value = r.Name,
					Selected = (r.Name == currentRole) // Mark the current role as selected in the dropdown
				})
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return NotFound(); 
				}

				// Update user properties with data from the form
				user.FullName = model.FullName;
				user.Email = model.Email;

				// If you want to update the user's role, you can do so here
				if (!string.IsNullOrEmpty(model.SelectedRole))
				{
					var userRoles = await _userManager.GetRolesAsync(user);
					await _userManager.RemoveFromRolesAsync(user, userRoles);
					await _userManager.AddToRoleAsync(user, model.SelectedRole);
				}

				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					// Redirect to a success page or return JSON indicating success
					return RedirectToAction("Index", "User", new { area = "Admin"});
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			// If we got this far, something failed, redisplay form with validation errors
			var roles = await _roleManager.Roles.ToListAsync();
			model.AvailableRoles = roles.Select(r => new SelectListItem
			{
				Text = r.Name,
				Value = r.Name
			});
			return View(model);
		}
	}
}
