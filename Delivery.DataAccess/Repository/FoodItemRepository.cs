using DeliveryAll.DataAccess.Data;
using DeliveryAll.DataAccess.Repository;
using DeliveryAll.DataAccess.Repository.IRepository;
using DeliveryAll.Models;
using DeliveryAll.Repository.IRepository;

namespace DeliveryAll.DataAccess.Repository
{
	public class FoodItemrepository : Repository<FoodItem>, IFoodItemRepository
	{
		private ApplicationDbContext _db;
		public FoodItemrepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FoodItem obj)
		{
			var objFromdb = _db.FoodItems.FirstOrDefault(u => u.Id == obj.Id);
			if(objFromdb != null)
			{
				objFromdb.Name = obj.Name;
				objFromdb.Description = obj.Description;
				objFromdb.Price = obj.Price;
				objFromdb.CategoryId = obj.CategoryId;
				objFromdb.FoodItemImages = obj.FoodItemImages;
				//if(obj.ImageUrl != null)
				//{
				//	objFromdb.ImageUrl = obj.ImageUrl;
				//}
			}
		}
	}
}
