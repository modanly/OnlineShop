using OnlineShop.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {

        private readonly IRolesRepository rolesRepository;

        public RoleController(IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }


        [Area("Admin")]
        public IActionResult Index()
        {
            var roles = rolesRepository.GetAllRoles();
            return View(roles);
        }
        [Area("Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult Add(Roles role)
        {
            var roles = rolesRepository.GetAllRoles();
            if (roles.FirstOrDefault(r => r.Name == role.Name) != null)
            {
                ModelState.AddModelError("", "Такая роль уже есть");
            }
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            rolesRepository.Add(role);
            return RedirectToAction("Index");
        }



        [Area("Admin")]
        [HttpPost]
        public IActionResult Del(string Name)
        {
            var roles = rolesRepository.GetAllRoles();
            var currentRole = roles.FirstOrDefault(role => role.Name == Name);
            rolesRepository.Del(currentRole);
            return RedirectToAction("Index");
        }

    }
}
