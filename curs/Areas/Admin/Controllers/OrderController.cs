using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area(Constans.AdminRoleName)]
   // [Authorize(Roles =Constans.AdminRoleName)]
    public class OrderController : Controller
    {
        
        private readonly IOrdersRepository ordersRepository;
        

        public OrderController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;   
        }
        

        
        public async Task<IActionResult> Index()
        {
            var orders = await ordersRepository.GetAllOrdersAsync();
            return View(orders.Select(x=>Mapping.ToOrderViewModel(x)).ToList());
        }

        
        public async Task<IActionResult> DetailAsync(Guid id)
        {
            var order = await ordersRepository.TryGetByIdAsync(id);

            return View(Mapping.ToOrderViewModel(order));
        }

        [HttpPost]
        
        public async Task<IActionResult> UpdateOrderStatusAsync(Guid Id, OrderStatusesViewModel Status)
        {
            await ordersRepository.UpdateStatusAsync(Id, (OrderStatuses)(int)Status);
            return RedirectToAction("Index");
        }

    }
}
