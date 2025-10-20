using Microsoft.AspNetCore.Mvc;

namespace AccommodationBooking.Api.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
