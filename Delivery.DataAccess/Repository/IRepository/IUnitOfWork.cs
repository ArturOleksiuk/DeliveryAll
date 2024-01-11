using DeliveryAll.DataAccess.Repository.IRepository;

namespace DeliveryAll.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		IFoodItemRepository FoodItem { get; }
		ICartRepository Cart { get; }
        IApplicationUserRepository ApplicationUser { get; }
		IOrderDetailRepository OrderDetail { get; }
		IOrderHeaderRepository OrderHeader { get; }
		IFoodItemImageRepository FoodItemImage { get; }
        void Save();
    }
}
