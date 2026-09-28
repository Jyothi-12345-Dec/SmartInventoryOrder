using Microsoft.EntityFrameworkCore;
using SmartInventory.Models;

namespace SmartInventory.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 2001,
                    Name = "Laptop",
                    Price = 50000,
                    Stock = 20
                },
                new Product
                {
                    Id = 2002,
                    Name = "Mobile",
                    Price = 30000,
                    Stock = 30
                },
                new Product
                {
                    Id = 2003,
                    Name = "Keyboard",
                    Price = 1500,
                    Stock = 50
                },
                new Product
                {
                    Id = 2004,
                    Name = "Mouse",
                    Price = 800,
                    Stock = 40
                }
            );


        }
    }
}