using OnlineShop.Db;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Helper;
using OnlineShop.Db.Models;

namespace OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartsRepository cartsRepository;
        private readonly IOrdersRepository ordersRepository;

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
        {
            this.cartsRepository = cartsRepository;
            this.ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Buy(UserDeliveryInfoViewModel user)
        {
            if (!user.Name.All(c => char.IsLetter(c) || c == ' '))
            {
                ModelState.AddModelError("", "ФИО должны содержать только буквы");
            }
            if (!user.Phone.All(c => char.IsDigit(c) || "+()- ".Contains(c)))
            {
                ModelState.AddModelError("", "Номер телефона может содержать только цифры и символы '+()-'");
            }
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var existingCart = cartsRepository.TryGetByUserId(Constans.UserId);
            
            var order = new Order
            {
                User = Mapping.ToUser(user),
                Items = existingCart.Items
            };
            ordersRepository.Add(order);
            cartsRepository.Clear(Constans.UserId);

            return View();
        }
    }
}
