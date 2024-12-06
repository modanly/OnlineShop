using OnlineShop.Helper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using System.Security.Claims;

namespace OnlineShop.Views.Shared.Component.Cart
{
    public class CartViewComponent:ViewComponent
    {
        private readonly ICartsRepository cartsRepository;
        private readonly UserManager<User> userManager;

        public CartViewComponent(ICartsRepository cartsRepository, UserManager<User> userManager)
        {
            this.cartsRepository = cartsRepository;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null)
            {
                // Если приведение не удалось, возвращаем пустое представление или ошибку
                return View("Cart", 0); // например, пустая корзина
            }
            var userId = userManager.GetUserId(claimsPrincipal);
            var cart = await cartsRepository.TryGetByUserIdAsync(userId);
            var cartViewModel = Mapping.ToCartViewModel(cart);
            var productCounts = cartViewModel?.Amount??0;
            
            return View("Cart", productCounts);

        }
       
    }
}
