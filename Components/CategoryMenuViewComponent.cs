using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fastwebsite.Entities;

namespace fastwebsite.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly FastdbContext _context;

        public CategoryMenuViewComponent(FastdbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
