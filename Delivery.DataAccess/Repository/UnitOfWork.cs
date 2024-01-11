using DeliveryAll.DataAccess.Data;
using DeliveryAll.DataAccess.Repository.IRepository;
using DeliveryAll.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAll.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;
		public ICategoryRepository Category { get; private set; }
        public IFoodItemRepository FoodItem { get; private set; }
		public ICartRepository Cart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }
		public IFoodItemImageRepository FoodItemImage { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			ApplicationUser = new ApplicationUserRepository(_db);
            Cart = new CartRepository(_db);
			Category = new CategoryRepository(_db);
			FoodItem = new FoodItemrepository(_db);
			OrderHeader = new OrderHeaderRepository(_db);
			OrderDetail = new OrderDetailRepository(_db);
			FoodItemImage = new FoodItemImageRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
    }
}
