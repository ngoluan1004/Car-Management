using BTL_Car.Data;
using BTL_Car.Helpers;
using BTL_Car.Models;
using BTL_Car.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace BTL_Car.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var count = HttpContext.Session.Get<List<Booking>>(MySetting.LISTBOOKING) ?? new List<Booking>();
                return View("CountBooking", new CartModel
                {
                    Quantity = count.Sum(p => p.booking_id)
                });
            } 


    }
}
