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
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Pizza", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Drinks", DisplayOrder = 2}
				);
            modelBuilder.Entity<FoodItem>().HasData(
                new FoodItem
                {
                    Id = 1,
                    Name = "PIZZA \"HAWAIIAN\" 490 G",
                    Description = "Signature sauce, mozzarella, marinated chicken, pineapple, mushrooms, tomatoes",
                    Price = 149,
					CategoryId = 1,
                    ImageUrl = ""
                },
                new FoodItem
                {
                    Id = 2,
                    Name = "PIZZA \"CHEESE\" 400 G",
                    Description = "Cream, mozzarella, brie, parmesan cheese",
                    Price = 149,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new FoodItem
                {
                    Id = 3,
                    Name = "PIZZA \"BARBECUE\" 550G",
                    Description = "Barbecue sauce, Mozzarella cheese, hunting sausages, pickled cucumber, french fries, ketchup",
                    Price = 159,
                    CategoryId = 1,
                    ImageUrl = ""
                },
				new FoodItem
				{
					Id = 4,
					Name = "PIZZA \"CARBONARA\" 420 G",
					Description = "Cream Sauce, mozzarella, ham, bacon, parmesan cheese, egg yolk",
					Price = 149,
					CategoryId = 1,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 5,
					Name = "PIZZA \"MARGHERITA\" 380 G",
					Description = "Signature sauce, mozzarella, tomatoes",
					Price = 120,
					CategoryId = 1,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 6,
					Name = "PIZZA \"MEAT\" 530 G",
					Description = "Soy BBQ, mozzarella, ham, bacon, hunting sausages, onions, tomatoes",
					Price = 150,
					CategoryId = 1,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 7,
					Name = "PIZZA \"PEPPERONI\" 450 G",
					Description = "Signature sauce, mozzarella, pepperoni, tomatoes",
					Price = 179,
					CategoryId = 1,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 8,
					Name = "FANTA",
					Description = "0,5l",
					Price = 23,
					CategoryId = 2,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 9,
					Name = "SPRITE",
					Description = "0,5l",
					Price = 23,
					CategoryId = 2,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 10,
					Name = "COCA COLA",
					Description = "0,5l",
					Price = 23,
					CategoryId = 2,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 11,
					Name = "APPLE JUICE",
					Description = "1l",
					Price = 62,
					CategoryId = 2,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 12,
					Name = "CHERRY JUICE",
					Description = "1l",
					Price = 62,
					CategoryId = 2,
					ImageUrl = ""
				},
				new FoodItem
				{
					Id = 13,
					Name = "TOMATO JUICE",
					Description = "1l",
					Price = 62,
					CategoryId = 2,
					ImageUrl = ""
				}
				);
        }
        
    }
}
