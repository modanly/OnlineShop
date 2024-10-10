
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

      //  private List<Product> products = new List<Product>()
      //{
      //    new Product("Поводок BullyBillows Swivel Combat, черный, 3 м", 6500, "- Стропа прошита неопреном\n- Поворотный механизм крепления\n- Специальный механизм замка «cobra»", "/images/image1.jpeg"),
      //    new Product("Ошейник Zee.Dog Blast, M, мультиколор", 2340, "- Двойная защита швов\n- 4-точечная система блокировки замка", "/images/image2.jpeg"),
      //    new Product("Ошейник Zee.Dog Sand, XS, бежевый", 1940, "- Двойная защита швов\n- 4-точечная система блокировки замка", "/images/image3.jpeg"),
      //    new Product("Шлейка Zee.Dog Mahalo, L, розовый", 4800, "- Прочный и мягкий полиэстер\n- 4-точечная система блокировки замка\n- Подходит для щенков\n- анатомическая конструкция", "/images/image4.jpeg")
      //};

        public void Add(Product product)
        {
            product.ImagePath = "/images/image1.jpeg";
            databaseContext.Products.Add(product);
            databaseContext.SaveChanges();
        }

        public List<Product> GetAll() 
        {
            return databaseContext.Products.ToList(); 
        }

        public Product TryGetById(Guid id)
        {
            return databaseContext.Products.FirstOrDefault(product => product.Id == id);

        }

        public void Update(Product product)
        {
            var existingProduct= databaseContext.Products.FirstOrDefault(x => x.Id == product.Id);
            if (existingProduct == null) 
            {
                return;
            }
            existingProduct.Name= product.Name;
            existingProduct.Description= product.Description;
            existingProduct.Cost= product.Cost;
            databaseContext.SaveChanges();
        }

        public void Del(Guid id)
        {
            var product = TryGetById(id);
            databaseContext.Products.Remove(product);
            databaseContext.SaveChanges();
        }

       
    }
}
