using OnlineShop.Db.Models;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new Login() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
                if (result.Succeeded)
                {
                    return Redirect(login.ReturnUrl ?? "/Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                }
            }
            return View(login);
        }

        public IActionResult Register(string returnUrl)
        {
            return View(new Register() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Register register)
        {

            if (register.UserName == register.Password)
            {
                ModelState.AddModelError("", "Имя пользователя и пароль не должны совпадать");
                return View(register);
            }
            if (ModelState.IsValid)
            {
                User user = new User { Email = register.UserName, UserName = register.UserName, PhoneNumber = register.Phone };
                //Добавление пользователя
                var result = await userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    //Установка куки
                    await  signInManager.SignInAsync(user, false);
                    TryAssignUserRoleAsync(user);
                    return Redirect(register.ReturnUrl ?? "/Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(register);
        }
        private async Task TryAssignUserRoleAsync(User user)
        {
            try
            {
                await userManager.AddToRoleAsync(user, Constans.UserRoleName);
            }
            catch
            {
                //log
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
