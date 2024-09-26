using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
