using OnlineShop.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area(Constans.AdminRoleName)]
    [Authorize(Roles = Constans.AdminRoleName)]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> rolesManager;

        public RoleController(RoleManager<IdentityRole> rolesManager)
        {
            this.rolesManager = rolesManager;
        }


        
        public IActionResult Index()
        {
            var roles = rolesManager.Roles.ToList();
            return View(roles.Select(x => new RolesViewModel { Name = x.Name }).ToList());
        }
        
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> AddAsync(RolesViewModel role)
        {
            var result=await rolesManager.CreateAsync(new IdentityRole(role.Name));
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }
            return View(role);
        }



        
        [HttpPost]
        public async Task<IActionResult> DelAsync(string roleName)
        {
            var role = await rolesManager.FindByNameAsync(roleName);
            if (role != null)
            {
                await rolesManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }



    }
}
