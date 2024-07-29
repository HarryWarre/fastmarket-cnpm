using fastwebsite.Entities;
using fastwebsite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace fastwebsite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FastdbContext _context;

        public OrdersController(FastdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.AccountId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách phương thức thanh toán
            ViewBag.PaymentMethods = await _context.TypePayments.ToListAsync();

            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int SelectedPaymentMethodId, string CouponCode)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.AccountId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == CouponCode && c.ExpirationDate >= DateTime.Now);

            if (coupon != null)
            {
                cart.TotalPrice -= (cart.TotalPrice * (coupon.Discount ?? 0) / 100);
            }

            var order = new Order
            {
                AccountId = userId,
                TotalPrice = cart.TotalPrice,
                OrderDate = DateTime.Now,
                State = "Pending",
                CodeCoupon = CouponCode,
                TypePaymentId = SelectedPaymentMethodId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderConfirmation", new { id = order.OrderId });
        }


        [HttpGet]
        public async Task<IActionResult> CheckCoupon(string code)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == code && c.ExpirationDate >= DateTime.Now);

            if (coupon != null)
            {
                return Json(new { valid = true, discount = coupon.Discount });
            }
            else
            {
                return Json(new { valid = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Index(string state)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders.Where(o => o.AccountId == userId);

            if (!string.IsNullOrEmpty(state))
            {
                orders = orders.Where(o => o.State == state);
            }

            ViewBag.SelectedState = state;

            return View(orders.ToList());
        }


        public async Task<IActionResult> OrderDetail(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
