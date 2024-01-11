using DeliveryAll.Models;

namespace DeliveryAll.Repository.IRepository
{
	public interface IFoodItemImageRepository : IRepository<FoodItemImage>
	{
		void Update(FoodItemImage obg);
		void Save();
	}
}
