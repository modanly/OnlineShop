
using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductsRepository productRepository;
        private readonly ICartsRepository cartsRepository;
        public HomeController(IProductsRepository productRepository, ICartsRepository cartsRepository)
        {
            this.productRepository = productRepository;
            this.cartsRepository = cartsRepository;
        }


        public IActionResult Index()
        {
            var products = productRepository.GetAll();
           
            return View(Mapping.ToProductViewModels(products));
        }

     
    }
}