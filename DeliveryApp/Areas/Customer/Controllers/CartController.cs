using DeliveryAll.Models;
using DeliveryAll.Models.ViewModels;
using DeliveryAll.Repository.IRepository;
using DeliveryAll.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace DeliveryApp.Areas.Customer.Controllers
{
    [Area("customer")]
    [Authorize]
    public class CartController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartVM = new()
            {
                CartList = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "FoodItem"),
                OrderHeader = new()
            };
            IEnumerable<FoodItemImage> foodItemImages = _unitOfWork.FoodItemImage.GetAll();
            
            foreach (var cart in CartVM.CartList)
            {
                cart.FoodItem.FoodItemImages = foodItemImages.Where(u => u.FoodItemId == cart.FoodItem.Id).ToList();
                cart.Price = cart.FoodItem.Price;
                CartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(CartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartVM = new()
            {
                CartList = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "FoodItem"),
                OrderHeader = new()
            };
            CartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            CartVM.OrderHeader.Name = CartVM.OrderHeader.ApplicationUser.Name;
            CartVM.OrderHeader.Phone = CartVM.OrderHeader.ApplicationUser.PhoneNumber;
            CartVM.OrderHeader.City = CartVM.OrderHeader.ApplicationUser.City;
            CartVM.OrderHeader.StreetAdress = CartVM.OrderHeader.ApplicationUser.StreetAddress;
            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.FoodItem.Price;
                CartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
			DateTime now = DateTime.Now;
			DateTime arrivalTime30Min = now.AddMinutes(30);
			DateTime arrivalTime60Min = now.AddMinutes(60);

			TimeSpan difference30Min = arrivalTime30Min - now;
			TimeSpan difference60Min = arrivalTime60Min - now;

			string arrivalTimeRange = $"{difference30Min.TotalMinutes} - {difference60Min.TotalMinutes} хв";

			// Передача у представлення
			ViewBag.ArrivalTimeRange = arrivalTimeRange;

			// Додатково, можна використати ViewBag для передачі самих дат, якщо потрібно
			ViewBag.ArrivalTime30Min = arrivalTime30Min;
			ViewBag.ArrivalTime60Min = arrivalTime60Min;

			return View(CartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartVM.CartList = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "FoodItem");
            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.FoodItem.Price;
                CartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            CartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            CartVM.OrderHeader.OrderStatus = SD.PaymentStatusPending;

            _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            _unitOfWork.Save();
            
            foreach (var cart in CartVM.CartList)
            {
                OrderDetail orderDetail = new()
                {
                    FoodItemId = cart.FoodItemId,
                    OrderHeaderId = CartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
			var domain = "https://localhost:7145/";
			var options = new Stripe.Checkout.SessionCreateOptions
			{
				SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={CartVM.OrderHeader.Id}",
				CancelUrl = domain + "customer/cart/index",
				LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
				Mode = "payment",
			};
			foreach (var item in CartVM.CartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100),
						Currency = "uah",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.FoodItem.Name
						}

					},
					Quantity = item.Count
				};
				options.LineItems.Add(sessionLineItem);

			}
			var service = new SessionService();
			Session session = service.Create(options);
			_unitOfWork.OrderHeader.UpdateStripePaymentID(CartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}
		public IActionResult OrderConfirmation(int id)
		{
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
				_unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();
			}
            HttpContext.Session.Clear();
            List<Cart> Carts = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.Cart.RemoveRange(Carts);
            _unitOfWork.Save();

			return View(id);
		}
		public IActionResult Plus(int cartId) {
            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.Cart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == cartId, tracked: true);
            if (cartFromDb.Count <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.Cart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
                _unitOfWork.Cart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.Cart.Update(cartFromDb);
            }
           
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == cartId,tracked:true);
           
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.Cart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

            _unitOfWork.Cart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
    }
}
