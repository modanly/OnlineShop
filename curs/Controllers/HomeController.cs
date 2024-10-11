
using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Models;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductsRepository productRepository;
        private readonly ICartsRepository cartsRepository;
        private readonly IMemoryCache cache;
        public HomeController(IProductsRepository productRepository, ICartsRepository cartsRepository, IMemoryCache cache)
        {
            this.productRepository = productRepository;
            this.cartsRepository = cartsRepository;
            this.cache = cache;
        }


        public IActionResult Index()
        {
            cache.TryGetValue<List<Product>>(Constans.KeyCacheAllProducts, out var products);
            
           
            return View(products.ToProductViewModels());
        }

     
    }
}