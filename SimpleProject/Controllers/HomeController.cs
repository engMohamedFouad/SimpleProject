using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;

namespace SimpleProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _applicationDbContext.Category.Include(x => x.Products.Take(5)).ToListAsync();
            return View(categories);
        }

    }
}
