using DeliveryAll.DataAccess.Data;
using DeliveryAll.DataAccess.Repository;
using DeliveryAll.Models;
using DeliveryAll.Repository.IRepository;

namespace DeliveryAll.DataAccess.Repository
{
	public class FoodItemImageRepository : Repository<FoodItemImage>, IFoodItemImageRepository
    {
		private ApplicationDbContext _db;
		public FoodItemImageRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Save()
		{
			_db.SaveChanges();
		}
		public void Update(FoodItemImage obj)
		{
			_db.FoodItemImages.Update(obj);
		}
	}
}
