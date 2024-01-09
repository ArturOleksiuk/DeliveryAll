using DeliveryAll.DataAccess.Data;
using DeliveryAll.Models;
using DeliveryAll.Models.ViewModels;
using DeliveryAll.Repository.IRepository;
using DeliveryAll.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Stripe;

namespace DeliveryAll.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagment(string userId)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == userId),
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                })
            };
            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;
            return View(RoleVM);
        }
        [HttpPost]
		public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
		{
			string RoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(x => x.Id == RoleId).Name;

            if(!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
				ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagmentVM.ApplicationUser.Id);
                _db.SaveChanges();
				_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
				_userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
			}
           
            return RedirectToAction("Index");
		}
		#region API CALLS
		[HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

            }

            return Json(new { data = objUserList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id) 
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u=>u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
