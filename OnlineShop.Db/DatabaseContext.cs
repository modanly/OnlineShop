using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Image> Images { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
        .ToTable("Image");
            modelBuilder.Entity<Image>().HasOne(p => p.Product).WithMany(p => p.Images).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);
            var product1Id = Guid.Parse("c5425e02-55f1-4590-af64-67ea3f8562c9");
            var product2Id = Guid.Parse("3a0475bc-810e-4d36-8f24-774e16b2a89c");
            var image1 = new Image
            {
                Id = Guid.Parse("6175de51-f5f6-4d7d-b158-2922f06ea791"),
                Url = "/images/Products/image1.jpeg",
                ProductId = product1Id
            };
            var image2 = new Image
            {
                Id = Guid.Parse("3262d0ad-ada6-404c-b2af-9a0b903acbfa"),
                Url = "/images/Products/image2.jpeg",
                ProductId = product2Id
            };

            modelBuilder.Entity<Image>().HasData(image1, image2);

            modelBuilder.Entity<Product>()
    .Property(p => p.ConcurrencyToken)
    .IsRowVersion();

            modelBuilder.Entity<Product>().HasData(new List<Product>()
            {
                new Product(product1Id, "Name1", 10, "DESC1"),
                new Product(product2Id, "Name2", 100, "DESC2")
            });
        }
    }
}
