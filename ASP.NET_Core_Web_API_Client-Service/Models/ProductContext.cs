using System;
using Microsoft.EntityFrameworkCore;
namespace ASP.NET_Core_Web_API_Client_Service.Models
{
    public class ProductContext : DbContext
    {
        //инициализация набора данных
        public DbSet<Product> Products { get; set; }

        //создание базы данных
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //создание уникального первичного ключа
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(u=>u.ProductID);
        }
    }
}
