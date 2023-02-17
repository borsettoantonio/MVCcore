using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    // Specifica dei valori per l'header della risposta HTTP
    // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    // oppure per tutte le action del controller
    // [ResponseCache(CacheProfileName ="Home")]
    public class HomeController : Controller
    {
        [ResponseCache(CacheProfileName ="Home")]
        public IActionResult Index()
        {
           return View();
        }
    }
}