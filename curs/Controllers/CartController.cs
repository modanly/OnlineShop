using OnlineShop.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductsRepository productRepository;
        private readonly ICartsRepository cartsRepository;

        public CartController(IProductsRepository productRepository, ICartsRepository cartRepository)
        {
            this.productRepository = productRepository;
            this.cartsRepository = cartRepository;
        }

        public IActionResult Index()
        {

            var cart = cartsRepository.TryGetByUserId(Constans.UserId);
            return View(Mapping.ToCartViewModel(cart));
        }
        public IActionResult Add(Guid productId)
        {
            var product = productRepository.TryGetById(productId);
            cartsRepository.Add(product, Constans.UserId); 
            return RedirectToAction("Index");
           
        }

        public IActionResult DecreaseAmount(Guid productId)
        {
            cartsRepository.DecreaseAmount(productId, Constans.UserId);
            return RedirectToAction("Index");

        }

        public IActionResult Clear() 
        {
            cartsRepository.Clear(Constans.UserId);
            return RedirectToAction("Index");
        }
    }
}
