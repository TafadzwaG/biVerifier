using Microsoft.AspNetCore.Mvc;

namespace biVerifier.Controllers
{
    public class LisensesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
