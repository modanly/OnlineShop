using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        
        private readonly IOrdersRepository ordersRepository;
        

        public OrderController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;   
        }
        

        [Area("Admin")]
        public IActionResult Index()
        {
            var orders = ordersRepository.GetAllOrders();
            return View(orders.Select(x=>Mapping.ToOrderViewModel(x)).ToList());
        }

        [Area("Admin")]
        public IActionResult Detail(Guid id)
        {
            var order = ordersRepository.TryGetById(id);

            return View(Mapping.ToOrderViewModel(order));
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult UpdateOrderStatus(Guid Id, OrderStatusesViewModel Status)
        {
            ordersRepository.UpdateStatus(Id, (OrderStatuses)(int)Status);
            return RedirectToAction("Index");
        }

    }
}
