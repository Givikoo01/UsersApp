using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Models;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (user.IsActive == false)
                    {
                        ModelState.AddModelError(string.Empty, "Your account is blocked. Please contact support.");
                        return View(model);
                    }
                }
                    //login
                var result = await signInManager.PasswordSignInAsync(model.UserName!, model.Password!, false, false);
     
                if (result.Succeeded)
                {
                    user.LastLoginTime = DateTime.Now;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    RegistrationTime = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}