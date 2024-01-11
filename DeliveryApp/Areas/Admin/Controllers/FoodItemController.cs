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
                foodItemVM.FoodItem = _unitOfWork.FoodItem.Get(u => u.Id == id, includeProperties: "FoodItemImages");
				return View(foodItemVM);
			}
        }
        [HttpPost]
        public IActionResult Upsert(FoodItemVM foodItemVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {

                if (foodItemVM.FoodItem.Id == 0)
                {
                    _unitOfWork.FoodItem.Add(foodItemVM.FoodItem);
                }
                else
                {
                    _unitOfWork.FoodItem.Update(foodItemVM.FoodItem);
                }

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(files != null)
                {
                    foreach(IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string foodItemPath = @"images\fooditems\fooditem-" + foodItemVM.FoodItem.Id;
                        string finalPath = Path.Combine(wwwRootPath, foodItemPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        FoodItemImage foodItemImage = new()
                        {
                            ImageUrl = @"\" + foodItemPath + @"\" + fileName,
                            FoodItemId = foodItemVM.FoodItem.Id

                        };
                        if(foodItemVM.FoodItem.FoodItemImages == null)
                        {
                            foodItemVM.FoodItem.FoodItemImages = new List<FoodItemImage>();
                        }
                        foodItemVM.FoodItem.FoodItemImages.Add(foodItemImage);
                    }
                    _unitOfWork.FoodItem.Update(foodItemVM.FoodItem);
                    _unitOfWork.Save();
                }
              
				
                TempData["success"] = "FoodItem created/updated succesfully";
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
        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.FoodItemImage.Get(u => u.Id == imageId);
            int foodItemId = imageToBeDeleted.FoodItemId;
            if(imageToBeDeleted != null)
            {
                if(!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                        imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.FoodItemImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Deleted succesfully";
			}
            return RedirectToAction(nameof(Upsert), new { id = foodItemId});
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

			string foodItemPath = @"images\fooditems\fooditem-" + id;
			string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, foodItemPath);

			if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach(string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);    
                }
				Directory.Delete(finalPath);
			}
				
			_unitOfWork.FoodItem.Remove(foodItemToBeDeleted);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Delete Succesful"});
            
        }

        #endregion
    }
}
