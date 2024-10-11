using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db;

namespace OnlineShop.Helper
{
    public class ProductsCache : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMemoryCache cache;
        public ProductsCache(IServiceProvider serviceProvider, IMemoryCache cache)
        {
            this.serviceProvider = serviceProvider;
            this.cache = cache;
        }

        //Вызывается при старте приложения
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Цикл запускается, пока приложение не закроется
            while(!stoppingToken.IsCancellationRequested)
            {
                CachingAllProducts();
                await Task.Delay(TimeSpan.FromMilliseconds(60000), stoppingToken);
            }
        }
        private void CachingAllProducts()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var products = databaseContext.Products.Include(x => x.Images).ToList();
                if (products != null)
                {
                    cache.Set(Constans.KeyCacheAllProducts, products);
                }
                foreach(var product in products)
                {
                    if (product != null)
                    {
                        cache.Set(product.Id, product);
                    }
                }

            }
        }
    }
}
