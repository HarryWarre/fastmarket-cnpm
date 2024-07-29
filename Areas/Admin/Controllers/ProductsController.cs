using fastwebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace fastwebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly FastdbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(FastdbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: admin/products/index
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: admin/products/create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            if (categories.Count > 0)
            {
                // Đặt danh mục đầu tiên làm mặc định
                ViewBag.CateId = new SelectList(categories, "CateId", "Name", categories[0].CateId);
            }
            else
            {
                ViewBag.CateId = new SelectList(categories, "CateId", "Name");
            }

            return View();
        }
        // POST: admin/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (true)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, truyền lại danh sách danh mục để người dùng có thể chọn lại
            var categories = _context.Categories.ToList();
            ViewBag.CateId = new SelectList(categories, "CateId", "Name", product.CateId);
            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var categories = _context.Categories.ToList();
            if (categories.Count == 0)
            {
                // Có thể log hoặc thông báo lỗi
                throw new Exception("No categories found.");
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = new SelectList(categories, "CateId", "Name", product.CateId);

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CateId = new SelectList(categories, "CateId", "Name", product.CateId);
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
