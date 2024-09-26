using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
