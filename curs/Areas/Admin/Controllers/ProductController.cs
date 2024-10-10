using curs.Helper;
using curs.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;

namespace curs.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository productRepository;

        public ProductController(IProductsRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        

        [Area("Admin")]
        public IActionResult Index()
        {
            var products = productRepository.GetAll();
            
            return View(Mapping.ToProductViewModels(products));

        }

        [Area("Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult Add(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                return View(product);
            }

            var productDb = new Product
            {
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description
            };
            productRepository.Add(productDb);
            return RedirectToAction("Index");
        }

        [Area("Admin")]
        public IActionResult Edit(Guid id)
        {
            var product = productRepository.TryGetById(id);
            return View(product);
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult Edit(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            var productDb = new Product
            {
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description
            };

            productRepository.Update(productDb);
            return RedirectToAction("Products");
        }

        [Area("Admin")]
        public IActionResult Del(Guid id)
        {
            productRepository.Del(id);
            return RedirectToAction("Index");
        }
    }
}
