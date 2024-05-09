using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class AboutUSController : Controller
    {
        public IActionResult Index()
        {
            return View("AboutUs");
        }
    }
}
