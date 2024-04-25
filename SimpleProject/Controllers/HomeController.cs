using Microsoft.AspNetCore.Mvc;
using SimpleProject.Models;

namespace SimpleProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = "mohamed";
            ViewData["Tilte"] = "Home";
            Product product = new Product() { Id = 1, Name = "Mohamed" };
            return View(product);
        }

    }
}
