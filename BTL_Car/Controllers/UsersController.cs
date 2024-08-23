using BTL_Car.Data;
using BTL_Car.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BTL_Car.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult TestDatabaseConnection()
        {
            var canConnect = _dbContext.Database.CanConnect();
            if (canConnect)
            {
                return Content("Connection successful!");
            }
            else
            {
                return Content("Connection failed!");
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Users userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên người dùng đã tồn tại chưa
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.username == userViewModel.username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại, vui lòng chọn tên khác.");
                    return View(userViewModel);
                }

                var user = new Users
                {
                    username = userViewModel.username,
                    email = userViewModel.email,
                    password = userViewModel.password,
                    full_name = userViewModel.full_name,
                    phone_number = userViewModel.phone_number,
                    address = userViewModel.address,
                    date_of_birth = userViewModel.date_of_birth,
                    iUserRoleID = 2
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home"); // Redirect to home page or another page after successful registration
            }
            return View(userViewModel);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(DangNhapViewModel model)
        {
            var user = _dbContext.Users.Where(x => x.username == model.Username 
                && x.password == model.Password).SingleOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.username);
                HttpContext.Session.SetInt32("ID", user.user_id);
                HttpContext.Session.SetInt32("Role", user.iUserRoleID);
                if (user.iUserRoleID == 1)
                {
                    return RedirectToAction("Index", "Cars");
                }
                else
                {
                    return RedirectToAction("Home", "Cars");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu sai";
            }

            return View();
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập
        }

        [Route("UserInfor")]
        public IActionResult UserInfor()
        {
            var getID = HttpContext.Session.GetInt32("Role");
            var user = _dbContext.Users.SingleOrDefault(u => u.user_id == getID);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Route("EditUser")]
        public IActionResult EditUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditUser")]
        public IActionResult EditUser(int id, [Bind("user_id,username,email,password,full_name,phone_number,address,date_of_birth,iUserRoleID")] Users user)
        {
            if (id != user.user_id)
            {
                return RedirectToAction("Index","Cars");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(user);
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.user_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Cars");
            }
            return View(user);
        }
        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(u => u.user_id == id);
        }
    }
}
