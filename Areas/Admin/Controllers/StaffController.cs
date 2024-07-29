using fastwebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fastwebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StaffController : Controller
    {
        private readonly FastdbContext _context;
        private readonly ILogger<StaffController> _logger;
        public StaffController(FastdbContext context, ILogger<StaffController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //// GET: StaffController
        //public ActionResult Index()
        //{
        //    var staffList = _context.SalesStaffs.ToList();
        //    return View(staffList);
        //}

        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Staff/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.SalesStaffs.SingleOrDefault(s => s.Email == email && s.Password == password);

            if (admin != null)
            {
                // Thiết lập session hoặc cookie cho phiên đăng nhập
                HttpContext.Session.SetString("AdminEmail", admin.Email);

                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    HttpContext.Session.Remove("ReturnUrl");
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Orders", new { area = "Admin" });
            }

            ViewBag.ErrorMessage = "Invalid login attempt";
            return View();
        }

        // POST: Admin/Staff/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }
    }
}
