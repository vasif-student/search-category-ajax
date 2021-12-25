using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_proj.Models;
using MVC_proj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_proj.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var dbUser = await _userManager.FindByNameAsync(model.UserName);

            if(dbUser != null)
            {
                ModelState.AddModelError(nameof(RegisterViewModel.UserName),
                    "The user with this username is already exist");
                return View();
            }

            User user = new User
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);
            if(!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
