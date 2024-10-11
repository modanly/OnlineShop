using OnlineShop.Db;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Helper;
using OnlineShop.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartsRepository cartsRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly UserManager<User> userManager;
        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository, UserManager<User> userManager)
        {
            this.cartsRepository = cartsRepository;
            this.ordersRepository = ordersRepository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyAsync(UserDeliveryInfoViewModel user)
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
            var userId = userManager.GetUserId(User);
            var existingCart = await cartsRepository.TryGetByUserIdAsync(userId);
            
            var order = new Order
            {
                User = Mapping.ToUser(user),
                Items = existingCart.Items
            };
            await ordersRepository.AddAsync(order);
            await cartsRepository.ClearAsync(userId);

            return View();
        }
    }
}
