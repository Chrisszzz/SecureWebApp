using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureWebApp.Data;
using SecureWebApp.Models;
using SecureWebApp.ViewModel;

namespace SecureWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _userData;
        public AccountController(IUser user)
        {
            _userData = user;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new Models.User
                    {
                        Username = registrationViewModel.Username,
                        Password = registrationViewModel.Password,
                        RoleName = "contributor"
                    };

                    _userData.Registration(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;

            }
            return View(registrationViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = loginViewModel.Username,
                        Password = loginViewModel.Password

                    };

                    var loginUser = _userData.Login(user);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };
                    var identity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = loginViewModel.RememberLogin
                        });
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(loginViewModel);
        }

        // GET: /Account/ChangePassword
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to change the password
                // Assuming you have a method to get the current user (adjust according to your user management)

                // Get the currently logged-in user (if using Identity)
                // var user = await _userManager.FindByNameAsync(User.Identity.Name);
                // var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                // Simulating the change password logic
                var user = _userData.Login(new User { Username = User.Identity.Name, Password = model.CurrentPassword });

                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    _userData.UpdateUserPassword(user); // Implement this method in IUser
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
            }
            return View(model);
        }
    }
}

