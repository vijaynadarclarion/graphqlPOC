using Microsoft.AspNetCore.Mvc;

namespace WebSocketClient.Controllers
{
    public class HomeController : Controller
    {
        // Action for the default Index page
        public IActionResult Index()
        {
            return View();
        }

        // Action for an About page (optional)
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        // Action for an Error page (optional)
        public IActionResult Error()
        {
            return View();
        }
    }
}
