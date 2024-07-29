using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fastwebsite.Entities;

namespace fastwebsite.Controllers
{
    public class SearchController : Controller
    {
        private readonly FastdbContext _context;

        public SearchController(FastdbContext context)
        {
            _context = context;
        }

        // GET: /Search/Results
        public async Task<IActionResult> Results(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return View(new List<Product>());
            }

            var products = await _context.Products
                .Where(p => p.ProductName.Contains(name))
                .ToListAsync();

            return View(products);
        }
    }
}
