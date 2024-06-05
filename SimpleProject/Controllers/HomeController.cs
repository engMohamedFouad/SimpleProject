using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;

namespace SimpleProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private int userId = 1;
        private string userName = "mohamed";
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index()
        {
            //CookieOptions cookieOptions = new CookieOptions();
            //cookieOptions.Expires = DateTime.UtcNow.AddSeconds(40);
            //Response.Cookies.Append("userId", userId.ToString(), cookieOptions);
            //Response.Cookies.Append("userName", userName, cookieOptions);

            //var product = new Product() { Id = 1, Name = "suger" };
            //HttpContext.Session.Set("product", JsonSerializer.SerializeToUtf8Bytes(product));

            //HttpContext.Session.SetInt32("userId", 1);
            //HttpContext.Session.SetString("userName", "Mohamed");

            //TempData["userId"] = 1;
            //TempData["userName"] = "Mohamed";

            var categories = await _applicationDbContext.Category.Include(x => x.Products.Take(5)).ToListAsync();
            return View(categories);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
