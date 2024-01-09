using DeliveryAll.DataAccess.Data;
using DeliveryAll.Models;
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
			}

            return;
        }
        
    }
}
