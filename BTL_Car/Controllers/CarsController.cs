using BTL_Car.Data;
using BTL_Car.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_Car.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CarsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // hiển thị danh sách car
        public IActionResult Index()
        {
            var cars = _dbContext.Cars.ToList();
            return View(cars);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("car_id,car_make,car_model,year_production,color,price_per_day,rating,license_plate,seats,transmission,fuel_type,image_url")] Cars car)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(car);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public IActionResult Edit(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("car_id,car_make,car_model,year_production,color,price_per_day,rating,license_plate,seats,transmission,fuel_type,image_url")] Cars car)
        {
            var id = HttpContext.Session.GetInt32("ID");
            if (id != car.car_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(car);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.car_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _dbContext.Cars.Any(e => e.car_id == id);
        }

        [HttpGet]
        public IActionResult Home()
        {
            var cars = _dbContext.Cars.ToList();
            return View(cars);
        }


        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("CarDetail")]
        public IActionResult CarDetail(int id)
        {
            var car = _dbContext.Cars.SingleOrDefault(c => c.car_id == id);
            if (car == null)
            {
                return NotFound();
            }

            var comments = _dbContext.Comments
                .Where(c => c.car_id == id)
                .ToList();

            var userNames = _dbContext.Users
                .ToDictionary(u => u.user_id, u => u.username);

            var viewModel = new CarDetailsViewModel
            {
                Car = car,
                Comments = comments
            };

            ViewBag.UserNames = userNames;

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddComment(int carId, string content, string? verifyKeyy)
        {
            
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Users");
            }
            
            var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var userId = user.user_id;


            var comment = new Comments
            {
                car_id = carId,
                content = content,
                user_id = userId ,
                VerifyKey = verifyKeyy
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return RedirectToAction("CarDetail", new { id = carId });
        }

        public IActionResult SortAlpha(){
            var cars = _dbContext.Cars.ToList();
            cars = cars.OrderBy(c => c.car_model).ToList();
            return View("Index",cars);
        }

        public IActionResult SortPrice(){
            var cars = _dbContext.Cars.ToList();
            cars = cars.OrderBy(c => c.price_per_day).ToList();
            return View("Index",cars);
        }

    public ActionResult Search(string carModel, int? pricePerDay, string color, int? yearProduction)
    {
        var cars = _dbContext.Cars.AsQueryable();

        if (!string.IsNullOrEmpty(carModel))
        {
            cars = cars.Where(c => c.car_model.Contains(carModel));
        }

        if (pricePerDay.HasValue)
        {
            cars = cars.Where(c => c.price_per_day == pricePerDay.Value);
        }

        if (!string.IsNullOrEmpty(color))
        {
            cars = cars.Where(c => c.color.Contains(color));
        }

        if (yearProduction.HasValue)
        {
            cars = cars.Where(c => c.year_production == yearProduction.Value);
        }

        return View("Index", cars.ToList()); // Render the Index view with filtered data
    }
    }
}
