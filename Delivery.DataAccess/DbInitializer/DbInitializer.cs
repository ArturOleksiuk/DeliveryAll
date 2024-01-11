using DeliveryAll.DataAccess.Data;
using DeliveryAll.Models;
using DeliveryAll.Models.ViewModels;
using DeliveryAll.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAll.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(
            UserManager<IdentityUser> user,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db){
            _userManager = user;
            _roleManager = roleManager;
            _db = db;

        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@dotnetmastery.com",
                    Email = "admin@dotnetmastery.com",
                    Name = "Artur Oleksiuk",
                    PhoneNumber = "0983453376",
                    StreetAddress = "Dovga",
                    City = "Ivano-Frankivsk"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user1 = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@dotnetmastery.com");
                _userManager.AddToRoleAsync(user1, SD.Role_Admin).GetAwaiter().GetResult();

				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "courier@dotnetmastery.com",
					Email = "courier@dotnetmastery.com",
					Name = "Robin",
					PhoneNumber = "0975434421",
					StreetAddress = "Doroshenka",
					City = "Ivano-Frankivsk"
				}, "Courier123*").GetAwaiter().GetResult();
				ApplicationUser user2 = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "courier@dotnetmastery.com");
				_userManager.AddToRoleAsync(user2, SD.Role_Employee).GetAwaiter().GetResult();

                var foodItemImage1 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\hawai-pizza.jpg",
                    FoodItemId = 1
                };
                _db.FoodItemImages.Add(foodItemImage1);
                var foodItemImage2 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-4Cheeses.jpg",
                    FoodItemId = 2
                };
                _db.FoodItemImages.Add(foodItemImage2);
                var foodItemImage3 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-barbeque.jpg",
                    FoodItemId = 3
                };
                _db.FoodItemImages.Add(foodItemImage3);
                var foodItemImage4 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-Carbonara.jpg",
                    FoodItemId = 4
                };
                _db.FoodItemImages.Add(foodItemImage4);
                var foodItemImage5 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-Margharita.jpg",
                    FoodItemId = 5
                };
                _db.FoodItemImages.Add(foodItemImage5);
                var foodItemImage6 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-Meat.jpg",
                    FoodItemId = 6
                };
                _db.FoodItemImages.Add(foodItemImage6);
                var foodItemImage7 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\pizza-Paperoni.jpg",
                    FoodItemId = 7
                };
                _db.FoodItemImages.Add(foodItemImage7);
                var foodItemImage8 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\fanta.jpg",
                    FoodItemId = 8
                };
                _db.FoodItemImages.Add(foodItemImage8);

                var foodItemImage9 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\sprite.jpg",
                    FoodItemId = 9
                };
                _db.FoodItemImages.Add(foodItemImage9);

                var foodItemImage10 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\cocacola.jpg",
                    FoodItemId = 10
                };
                _db.FoodItemImages.Add(foodItemImage10);

                var foodItemImage11 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\apple-juice.jpg",
                    FoodItemId = 11
                };
                _db.FoodItemImages.Add(foodItemImage11);
                var foodItemImage12 = new FoodItemImage
                {
                    ImageUrl = "\\images\\fooditem\\cherry-juice.jpg",
                    FoodItemId = 12
                };
                _db.FoodItemImages.Add(foodItemImage12);


                _db.SaveChanges();
            }
           

            return;
        }
        
    }
}
