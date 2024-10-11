using OnlineShop.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductsRepository productRepository;
        private readonly ICartsRepository cartsRepository;
        private readonly UserManager<User> userManager;

        public CartController(IProductsRepository productRepository, ICartsRepository cartRepository, UserManager<User> userManager)
        {
            this.productRepository = productRepository;
            this.cartsRepository = cartRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var cart = await cartsRepository.TryGetByUserIdAsync(userId);
            return View(Mapping.ToCartViewModel(cart));
        }
        public async Task<IActionResult> AddAsync(Guid productId)
        {
            var userId = userManager.GetUserId(User);
            var product = await productRepository.TryGetByIdAsync(productId);
            await cartsRepository.AddAsync(product, userId); 
            return RedirectToAction("Index");
           
        }

        public async Task<IActionResult> DecreaseAmountAsync(Guid productId)
        {
            var userId = userManager.GetUserId(User);
            await cartsRepository.DecreaseAmountAsync(productId, userId);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> ClearAsync() 
        {
            var userId=userManager.GetUserId(User);
            await cartsRepository.ClearAsync(userId);
            return RedirectToAction("Index");
        }
    }
}
