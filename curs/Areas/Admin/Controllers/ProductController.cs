using OnlineShop.Helper;
using OnlineShop.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OnlineShop.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area(Constans.AdminRoleName)]
    [Authorize(Roles = Constans.AdminRoleName)]
    public class ProductController : Controller
    {
        private readonly IProductsRepository productRepository;
        private readonly ImagesProvider imagesProvider;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductsRepository productRepository, ImagesProvider imagesProvider, ILogger<ProductController> logger)
        {
            this.productRepository = productRepository;
            this.imagesProvider = imagesProvider;
            _logger = logger;
        }


        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var products = await productRepository.GetAllAsync();

            return View(Mapping.ToProductViewModels(products));

        }

        [Area("Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> AddAsync(AddProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var imagesPaths = imagesProvider.SafeFiles(product.UploadedFiles, ImageFolders.Products);
            await productRepository.AddAsync(product.ToProduct(imagesPaths));
            return RedirectToAction(nameof(Index));
        }

        [Area("Admin")]
        public async Task<IActionResult> EditAsync(Guid productId)
        {
            var product = await productRepository.TryGetByIdAsync(productId);
            var productViewModel = product.ToEditProductViewModel();
            
            
            return View(productViewModel);
        }

        [HttpPost]
        [Area("Admin")]

        public async Task<IActionResult> EditAsync(EditProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = await productRepository.TryGetByIdAsync(product.Id);
                if (productToUpdate == null)
                {
                    ModelState.AddModelError(string.Empty, "Невозможно сохранить изменения, товар был удален другим пользователем.");
                    return View(product);
                }

                // Обработка загруженных файлов
                if (product.UploadedFiles != null && product.UploadedFiles.Any())
                {
                    var addedImagesPaths = imagesProvider.SafeFiles(product.UploadedFiles, ImageFolders.Products);
                    product.ImagesPaths.AddRange(addedImagesPaths);
                }

                // Преобразуем модель в объект продукта
                var updatedProduct = product.ToProduct();

                // Обновление данных продукта и его изображений
                await productRepository.UpdateAsync(updatedProduct);

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
        [Area("Admin")]
        public async Task<IActionResult> DelAsync(Guid productId)
        {
            await productRepository.DelAsync(productId);
            return RedirectToAction("Index");
        }
    }
}
