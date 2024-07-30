using fastwebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace fastwebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly FastdbContext _context;

        public AccountController(FastdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email đã tồn tại
                var existingCustomer = _context.Customers.SingleOrDefault(c => c.Email == customer.Email);
                if (existingCustomer == null)
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Email == email && c.Password == password);
            if (customer != null)
            {
                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetInt32("UserId", customer.AccountId);
                HttpContext.Session.SetString("UserEmail", customer.Email);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers.Find(userId.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public IActionResult Update()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers.Find(userId.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Update(Customer updatedCustomer)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers.Find(userId.Value);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.Phone = updatedCustomer.Phone;
            customer.Address = updatedCustomer.Address;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
