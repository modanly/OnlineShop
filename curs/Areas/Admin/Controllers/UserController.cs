using OnlineShop.Areas.Admin.Models;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using OnlineShop.Helper;
using OnlineShop.Db;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area(Constans.AdminRoleName)]
    [Authorize(Roles = Constans.AdminRoleName)]
    public class UserController : Controller
    {
        private readonly UserManager<User> usersManager;
        private readonly RoleManager<IdentityRole> rolesManager;

        public UserController(UserManager<User> usersManager, RoleManager<IdentityRole> rolesManager)
        {
            this.usersManager = usersManager;
            this.rolesManager = rolesManager;
        }


        public IActionResult Index()
        {
            var users = usersManager.Users.ToList();

            return View(users.Select(x => x.ToUserViewModel(usersManager)).ToList());
        }

        public async Task<IActionResult> DetailsAsync(string userName)
        {

            var user = await usersManager.FindByNameAsync(userName);
            return View(user.ToUserViewModel(usersManager));
        }

        public IActionResult ChangePassword(string userName)
        {
            var changePassword = new ChangePassword
            {
                UserName = userName
            };
            return View(changePassword);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword)
        {
            if (changePassword.UserName == changePassword.Password)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }
            if (ModelState.IsValid)
            {
                var user = await usersManager.FindByNameAsync(changePassword.UserName);
                //перенести в личный кабинет пользователя
                var newHashPassword = usersManager.PasswordHasher.HashPassword(user, changePassword.Password);
                user.PasswordHash = newHashPassword;
                await usersManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(changePassword));
        }



        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddUser addUser)
        {
            var userAccount = await usersManager.FindByNameAsync(addUser.UserName);
            if (userAccount != null)
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже есть.");
                return View(addUser);
            }
            if (addUser.UserName == addUser.Password)
            {
                ModelState.AddModelError("", "Имя пользователя и пароль не должны совпадать");
                return View(addUser);
            }
            if (ModelState.IsValid)
            {
                User user = new User { Email = addUser.UserName, UserName = addUser.UserName, PhoneNumber = addUser.Phone };
                //Добавление пользователя
                var result = await usersManager.CreateAsync(user, addUser.Password);
                if (result.Succeeded)
                {
                    await usersManager.AddToRoleAsync(user, Constans.UserRoleName);
                }
            }

            // usersRepository.Add(new User(register.UserName, register.Password, register.FirstName, register.LastName, register.Phone));


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DelAsync(string userName)
        {
            var user = await usersManager.FindByNameAsync(userName);
            await usersManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> EditAsync(string userId)
        {
            var user = await usersManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var editUser = new EditUser
            {
                UserName = user.UserName,
                Phone = user.PhoneNumber
            };
            return View(editUser);
        }


        [HttpPost]
        public async Task<IActionResult> EditAsync(string userId, EditUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await usersManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }


                user.UserName = model.UserName;
                user.PhoneNumber = model.Phone;

                var result = await usersManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }


        public async Task<IActionResult> EditRightsAsync(string userId)
        {
            var user = await usersManager.FindByIdAsync(userId);
            var roles = rolesManager.Roles.ToList();
            var assignedRoles = await usersManager.GetRolesAsync(user);
            var model = new EditRightsViewModel
            {
                Id = userId,
                UserName = user.UserName,

                Roles = roles.Select(x => new RolesViewModel { Name = x.Name }).ToList(),
                AssignedRoles = assignedRoles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRightsAsync(string userId, List<string> roleNames)
        {

            var user = await usersManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Если пользователь не найден
            }

            // Получаем текущие роли пользователя
            var currentRoles = await usersManager.GetRolesAsync(user);

            // Удаляем все текущие роли пользователя
            var removeResult = await usersManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Не удалось удалить текущие роли.");
                return View(); // Возвращаем представление с ошибкой
            }

            // Добавляем новые роли
            if (roleNames != null && roleNames.Any()) // Проверяем, были ли выбраны роли
            {
                var addResult = await usersManager.AddToRolesAsync(user, roleNames);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Не удалось добавить роли.");
                    return View();
                }
            }

            return RedirectToAction("Index");
        }
    }

}

