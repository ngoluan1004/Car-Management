using Microsoft.AspNetCore.Mvc;
using BTL_Car.Models;
using BTL_Car.Data;
using System.Linq;
using System.Threading.Tasks;
using BTL_Car.Helpers;
using NuGet.Protocol;
using System.Security.Policy;

namespace BTL_Car.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _dbContext;
        const string LISTBOOKING = "MY_LIST";
        public BookingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult Book(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = new BookingViewModel
            {
                CarId = car.car_id,
                CarMake = car.car_make,
                CarModel = car.car_model,
                BookingDate = DateTime.Now
            };

            return View(viewModel);
        }
        /*
        public IActionResult Book([Bind("CarId, RentalStartDate, RentalEndDate")] BookingViewModel bookingViewModel)
         {
             if (bookingViewModel == null)
             {
                 ModelState.AddModelError("", "Dữ liệu đặt xe không hợp lệ.");
                 return View(bookingViewModel);
             }

             // Kiểm tra CarId
             if (bookingViewModel.CarId <= 0)
             {
                 ModelState.AddModelError("", "ID của xe không hợp lệ.");
                 return View(bookingViewModel);
             }

             var car = _dbContext.Cars.Find(bookingViewModel.CarId);
             if (car == null)
             {
                 ModelState.AddModelError("", "Xe không tồn tại.");
                 return View(bookingViewModel);
             }

             // Kiểm tra tính khả dụng của xe
             var existingBooking = _dbContext.Bookings
                 .Any(b => b.car_booking_id == bookingViewModel.CarId &&
                           ((b.rental_start_date <= bookingViewModel.RentalEndDate) && (b.rental_end_date >= bookingViewModel.RentalStartDate)));

             if (existingBooking)
             {
                 ModelState.AddModelError("", "Xe đã được đặt trong khoảng thời gian này.");
                 return View(bookingViewModel);
             }

             // Tính toán tổng chi phí
             var rentalDays = (bookingViewModel.RentalEndDate - bookingViewModel.RentalStartDate).Days;
             var totalCost = rentalDays * car.price_per_day;

             // Lấy thông tin người dùng
             var userName = GetCurrentUserName(); // Thay thế GetCurrentUserName bằng phương thức thực tế của bạn
             var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

             if (user == null)
             {
                 ModelState.AddModelError("", "Không thể xác định người dùng.");
                 return View(bookingViewModel);
             }

             int userId = user.user_id;
             // Tạo mới booking
             var booking = new Booking
             {
                 user_booking_id = userId,
                 car_booking_id = bookingViewModel.CarId,
                 booking_date = DateTime.Now,
                 rental_start_date = bookingViewModel.RentalStartDate,
                 rental_end_date = bookingViewModel.RentalEndDate,
                 total_cost = totalCost,
                 status_car = "Đã đặt"
             };

             try
             {
                 _dbContext.Bookings.Add(booking);
                 _dbContext.SaveChanges();

                 // Thêm thông báo thành công vào TempData
                 TempData["SuccessMessage"] = "Đặt xe thành công!";

             }
             catch (Exception ex)
             {
                 ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                 //return View(bookingViewModel);
                 return RedirectToAction("Index", "Home");
             }

             return RedirectToAction("ListBooking");
         }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(BookingViewModel booking)
        {
            if (booking == null)
            {
                ModelState.AddModelError("", "Dữ liệu đặt xe không hợp lệ.");
                return View(booking);
            }

            // Kiểm tra CarId
            if (booking.CarId <= 0)
            {
                ModelState.AddModelError("", "ID của xe không hợp lệ.");
                return View(booking);
            }

            var car = _dbContext.Cars.Find(booking.CarId);
            if (car == null)
            {
                ModelState.AddModelError("", "Xe không tồn tại.");
                return View(booking);
            }
            
            if (ModelState.IsValid) {

                // Kiểm tra tính khả dụng của xe
                var existingBooking = _dbContext.Bookings
                    .Any(b => b.car_booking_id == booking.CarId &&
                              ((b.rental_start_date <= booking.RentalEndDate) && (b.rental_end_date >= booking.RentalStartDate)));

                if (existingBooking)
                {
                    ModelState.AddModelError("", "Xe đã được đặt trong khoảng thời gian này.");
                    return View(booking);
                }

                // Tính toán tổng chi phí
                var rentalDays = (booking.RentalEndDate - booking.RentalStartDate).Days;
                var totalCost = rentalDays * car.price_per_day;
                // Lấy thông tin người dùng
                var userName = HttpContext.Session.GetString("UserName"); // Thay thế GetCurrentUserName bằng phương thức thực tế của bạn
                var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

                if (user == null)
                {
                    ModelState.AddModelError("", "Không thể xác định người dùng vui lòng đăng nhập .");
                    return View(booking);
                }

                int userId = user.user_id;

                var item = new Booking
                {

                    user_booking_id = userId,
                    car_booking_id = booking.CarId,
                    booking_date = DateTime.Now,
                    rental_start_date = booking.RentalStartDate,
                    rental_end_date = booking.RentalStartDate,
                    total_cost = (decimal)totalCost,
                    status_car = "Đã đặt"
                };
                try
                {
                    _dbContext.Bookings.Add(item);
                    _dbContext.SaveChanges();

                    // Thêm thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đặt xe thành công!";
                    return RedirectToAction("hiendsBooking");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                    //return RedirectToAction("Book");
                    return View(booking);
                }
            }
            return View(booking); ;
        }
        public IActionResult hiendsBooking()
        {
            var userName = HttpContext.Session.GetString("UserName"); // Thay thế GetCurrentUserName bằng phương thức thực tế của bạn
            var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);
   
            if (user == null)
            {
                return RedirectToAction("Login","Users");
            }  

            var list = _dbContext.Bookings.Where(x=>x.user_booking_id==user.user_id).ToList();
            return View(list);
        }
        private string GetCurrentUserName()
        {
            // Giả sử bạn lưu tên người dùng trong session
            return HttpContext.Session.GetString("UserName");
        }


        // GET: Booking/Cancel/5
        public IActionResult Cancel(int id)
        {
            var booking = _dbContext.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChanges();
            //TempData["Cancel"] = "Hủy thành công!";
            return RedirectToAction("hiendsBooking");
        }
        [HttpGet]
        public IActionResult Pay(int id)
        {
            var booking = _dbContext.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            var item = new Booking
            {

                user_booking_id = booking.user_booking_id,
                car_booking_id = booking.car_booking_id,
                booking_date = DateTime.Now,
                rental_start_date = booking.rental_start_date,
                rental_end_date = booking.rental_end_date,
                total_cost = (decimal)booking.total_cost,
                status_car = "Đã thanh toán"
            };
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pay(Booking bookingPay)
        {

            if (ModelState.IsValid)
            {
                _dbContext.Update(bookingPay);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("hiendsBooking");
        }



        /*// POST: Booking/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelConfirmed(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }*/


        public IActionResult Index()
        {
           var cars = _dbContext.Cars.ToList(); // Lấy danh sách xe từ cơ sở dữ liệu
           //var cars =_dbContext.Bookings.ToList();
            return View(cars);
        }
        
    }
}
