using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Host.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
