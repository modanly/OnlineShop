using curs.Helper;
using curs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;

namespace curs.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository productRepository;
        public ProductController(IProductsRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public IActionResult Index(Guid id)
        {
            var product = productRepository.TryGetById(id);
            return View(Mapping.ToProductViewModel(product));
           
        }
        

      
    }
}
