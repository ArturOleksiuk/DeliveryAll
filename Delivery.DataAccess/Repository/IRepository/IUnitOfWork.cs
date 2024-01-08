using DeliveryAll.DataAccess.Repository.IRepository;

namespace DeliveryAll.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		IFoodItemRepository FoodItem { get; }
		ICartRepository Cart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
	}
}
