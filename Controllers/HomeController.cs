using fastwebsite.Entities;
using fastwebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace fastwebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly FastdbContext _context;

        public HomeController(FastdbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Category(int id)
        {
            var products = _context.Products.Where(p => p.CateId == id).ToList();
            var category = _context.Categories.FirstOrDefault(c => c.CateId == id);

            if (category != null)
            {
                ViewBag.Category = category.Name;
            }
            else
            {
                ViewBag.Category = "Unknown Category";
            }

            LoadCategories();  // Gọi phương thức LoadCategories

            return View(products);
        }

        // Phương thức này sẽ lấy danh sách các danh mục và truyền vào ViewBag
        private void LoadCategories()
        {
            ViewBag.Categories = _context.Categories.ToList();
        }

        public IActionResult ProductDetail(int id)
        {
            var product = _context.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Account)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            // Kiểm tra nếu user đã mua sản phẩm
            var hasPurchased = _context.Orders
            .Any(o => o.AccountId == userId && o.OrderItems.Any(oi => oi.ProductId == id));

            // Kiểm tra nếu user đã đánh giá sản phẩm
            var hasReviewed = _context.Reviews
                .Any(r => r.AccountId == userId && r.ProductId == id);

            ViewBag.HasPurchased = hasPurchased;
            ViewBag.HasReviewed = hasReviewed;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(Review model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // Kiểm tra nếu user đã mua sản phẩm
            var hasPurchased = _context.Orders
            .Any(o => o.AccountId == userId && o.OrderItems.Any(oi => oi.ProductId == model.ProductId));

            // Kiểm tra nếu user đã đánh giá sản phẩm
            var hasReviewed = _context.Reviews
                .Any(r => r.AccountId == userId && r.ProductId == model.ProductId);

            if (!hasPurchased || hasReviewed)
            {
                return BadRequest("Bạn không thể đánh giá sản phẩm này.");
            }

            var review = new Review
            {
                AccountId = (int)userId!,
                ProductId = model.ProductId,
                Content = model.Content,
                Rates = model.Rates,
                Account = await _context.Customers.FindAsync(userId),
                Product = await _context.Products.FindAsync(model.ProductId)
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProductDetail", new { id = model.ProductId });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
