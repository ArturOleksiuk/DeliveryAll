using DeliveryAll.DataAccess.Data;
using DeliveryAll.DataAccess.Repository;
using DeliveryAll.Models;
using DeliveryAll.Repository.IRepository;

namespace DeliveryAll.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
		private ApplicationDbContext _db;
		public OrderHeaderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Save()
		{
			_db.SaveChanges();
		}
		public void Update(OrderHeader obj)
		{
            _db.OrderHeaders.Update(obj);
		}
		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderfromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderfromDb != null)
            {
				orderfromDb.OrderStatus = orderStatus;
				if(!string.IsNullOrEmpty(paymentStatus))
				{
					orderfromDb.PaymentStatus = paymentStatus;
				}
            }
        }
		public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if(!string.IsNullOrEmpty(sessionId))
			{
				orderFromDb.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				orderFromDb.PaymentIntentId = paymentIntentId;
				orderFromDb.PaymentDate = DateTime.Now;
			}
		}
	}
}
