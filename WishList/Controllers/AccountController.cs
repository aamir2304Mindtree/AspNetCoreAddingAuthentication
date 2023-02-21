using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
               
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register", new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
               var result = _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    PasswordHash = registerViewModel.Password
                }, registerViewModel.Password).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("Password", error.Description);
                     }
                    return View("Register", registerViewModel);
                }
            }
            else
            {
                return View("Register", registerViewModel);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
