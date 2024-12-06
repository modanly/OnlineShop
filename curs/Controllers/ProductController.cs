using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Models;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository productRepository;
        private readonly IMemoryCache cache;

        public ProductController(IProductsRepository productRepository, IMemoryCache cache)
        {
            this.productRepository = productRepository;
            this.cache = cache;
        }
        public IActionResult Index(Guid productId)
        {
            cache.TryGetValue<Product>(productId, out var product);

            return View(product.ToProductViewModel());

        }



    }
}
