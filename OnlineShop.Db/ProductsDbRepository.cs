
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System.Linq;

namespace OnlineShop.Db
{
    public class ProductsDbRepository : IProductsRepository
    {
        private readonly DatabaseContext databaseContext;
        public ProductsDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }



        public async Task AddAsync(Product product)
        {

            databaseContext.Products.AddAsync(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await databaseContext.Products.Include(x => x.Images).ToListAsync();
        }

        public async Task<Product> TryGetByIdAsync(Guid productId)
        {
            return await databaseContext.Products.Include(x => x.Images).FirstOrDefaultAsync(product => product.Id == productId);

        }
        public async Task UpdateAsync(Product product)
        {
          
            
            var existingProduct = await TryGetByIdAsync(product.Id);



           
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Cost = product.Cost;


            // Обновление изображений
            foreach (var image in product.Images)
            {
                if (!existingProduct.Images.Any(i => i.Url == image.Url))
                {
                    existingProduct.Images.Add(image);
                }
            }

            await databaseContext.SaveChangesAsync();
        }
        //public async Task UpdateAsync(Product product)
        //{
        //    var existingProduct = await databaseContext.Products.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == product.Id);

        //    if (existingProduct == null)
        //    {
        //        return;
        //    }

        //    // Устанавливаем оригинальное значение ConcurrencyToken перед обновлением
        //    databaseContext.Entry(existingProduct).Property("ConcurrencyToken").OriginalValue = product.ConcurrencyToken;

        //    existingProduct.Name = product.Name;
        //    existingProduct.Description = product.Description;
        //    existingProduct.Cost = product.Cost;

        //    foreach (var image in product.Images)
        //    {
        //        image.ProductId = product.Id;
        //        databaseContext.Images.Add(image);
        //    }

        //    await databaseContext.SaveChangesAsync();
        //}

        public async Task DelAsync(Guid id)
        {
            var product = await TryGetByIdAsync(id);
            databaseContext.Products.Remove(product);
            await databaseContext.SaveChangesAsync();
        }


    }
}
