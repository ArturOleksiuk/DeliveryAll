using DeliveryAll.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAll.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Product", DisplayOrder = 2},
				new Category { Id = 3, Name = "Prod", DisplayOrder = 3 }
				);
            modelBuilder.Entity<FoodItem>().HasData(
                new FoodItem
                {
                    Id = 1,
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with tomato, mozzarella, and basil.",
                    Price = 123.99,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new FoodItem
                {
                    Id = 2,
                    Name = "Spaghetti Bolognese",
                    Description = "Italian pasta with meat sauce.",
                    Price = 69.99,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new FoodItem
                {
                    Id = 3,
                    Name = "Caesar Salad",
                    Description = "Fresh salad with romaine lettuce, croutons, and Caesar dressing.",
                    Price = 7.49,
                    CategoryId = 3,
                    ImageUrl = ""
                }
                );
        }
        
    }
}
