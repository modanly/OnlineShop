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
            productViewModel.ConcurrencyToken = product.ConcurrencyToken;
            
            return View(productViewModel);
        }

        [HttpPost]
        [Area("Admin")]
        [ValidateAntiForgeryToken]
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

                // Логируем перед тем, как сохранить
                _logger.LogInformation($"Конкуренция: старое значение ConcurrencyToken: {Convert.ToBase64String(productToUpdate.ConcurrencyToken)}");
                _logger.LogInformation($"Конкуренция: переданное значение ConcurrencyToken: {Convert.ToBase64String(product.ConcurrencyToken)}");

                try
                {
                    await productRepository.UpdateAsync(product.ToProduct());
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Ошибка конкурентности при сохранении данных продукта.");

                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Product)exceptionEntry.Entity;
                    var databaseEntry = await exceptionEntry.GetDatabaseValuesAsync();

                    if (databaseEntry == null)
                    {
                        _logger.LogInformation($"Продукт с Id {product.Id} был удален другим пользователем.");
                        ModelState.AddModelError(string.Empty, "Невозможно сохранить изменения, товар был удален другим пользователем.");
                    }
                    else
                    {
                        var databaseValues = (Product)databaseEntry.ToObject();

                        _logger.LogInformation($"Конкуренция: актуальные значения для продукта с Id {product.Id}: Name = {databaseValues.Name}, Cost = {databaseValues.Cost}");

                        // Применяем актуальные значения, если данные изменены
                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Текущее значение: {databaseValues.Name}");
                        }
                        if (databaseValues.Cost != clientValues.Cost)
                        {
                            ModelState.AddModelError("Cost", $"Текущее значение: {databaseValues.Cost}");
                        }
                        if (databaseValues.Description != clientValues.Description)
                        {
                            ModelState.AddModelError("Description", $"Текущее значение: {databaseValues.Description}");
                        }

                        // Обновляем токен для повторной отправки
                        product.ConcurrencyToken = databaseValues.ConcurrencyToken;
                        ModelState.Remove("ConcurrencyToken");

                        // Сообщаем о том, что данные были изменены другим пользователем
                        ModelState.AddModelError(string.Empty, "Данные были изменены другим пользователем.");
                    }
                }
            }

            // Обработка загруженных файлов
            if (product.UploadedFiles != null && product.UploadedFiles.Any())
            {
                var addedImagesPaths = imagesProvider.SafeFiles(product.UploadedFiles, ImageFolders.Products);
                product.ImagesPaths.AddRange(addedImagesPaths);
            }

            // Повторное обновление данных (когда ошибок нет)
            await productRepository.UpdateAsync(product.ToProduct());
            return RedirectToAction(nameof(Index));
        }

        [Area("Admin")]
        public async Task<IActionResult> DelAsync(Guid productId)
        {
            await productRepository.DelAsync(productId);
            return RedirectToAction("Index");
        }
    }
}
