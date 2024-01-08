using DeliveryAll.Models;
using DeliveryAll.Models.ViewModels;
using DeliveryAll.Repository.IRepository;
using DeliveryAll.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileSystemGlobbing;

namespace DeliveryApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class FoodItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FoodItemController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment; 
		}
        public IActionResult Index()
        {
            List<FoodItem> obgFoodItemList = _unitOfWork.FoodItem.GetAll(includeProperties:"category").ToList();
            return View(obgFoodItemList);
        }
        public IActionResult Upsert(int? id)
        {
            FoodItemVM foodItemVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                FoodItem = new FoodItem()
            };
            if(id == null || id == 0)
            {
                return View(foodItemVM);
            }
            else
            {
                foodItemVM.FoodItem = _unitOfWork.FoodItem.Get(u => u.Id == id);
				return View(foodItemVM);
			}
        }
        [HttpPost]
        public IActionResult Upsert(FoodItemVM foodItemVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string foodItemPath = Path.Combine(wwwRootPath, @"images\foodItem");
                    if(!string.IsNullOrEmpty(foodItemVM.FoodItem.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, foodItemVM.FoodItem.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    
                    using (var fileStream = new FileStream(Path.Combine(foodItemPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    foodItemVM.FoodItem.ImageUrl = @"\images\foodItem\" + fileName;
				}
              
                if(foodItemVM.FoodItem.Id == 0)
                {
					_unitOfWork.FoodItem.Add(foodItemVM.FoodItem);
				}
                else
                {
					_unitOfWork.FoodItem.Update(foodItemVM.FoodItem);
				}
				_unitOfWork.Save();
                TempData["success"] = "FoodItem created succesfully";
                return RedirectToAction("Index");
            }
            else
            {
				foodItemVM.CategoryList = _unitOfWork.Category
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    });
                return View(foodItemVM);

            }
        }
      
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<FoodItem> obgFoodItemList = _unitOfWork.FoodItem.GetAll(includeProperties: "category").ToList();
            return Json(new { data = obgFoodItemList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var foodItemToBeDeleted = _unitOfWork.FoodItem.Get(u => u.Id == id);
            if (foodItemToBeDeleted == null) 
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                foodItemToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.FoodItem.Remove(foodItemToBeDeleted);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Delete Succesful"});
            
        }

        #endregion
    }
}
