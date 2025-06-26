using Microsoft.AspNetCore.Mvc;

namespace MVCProject_ITI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
