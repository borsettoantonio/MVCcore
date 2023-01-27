using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return Content("<html><body>Sono <h1>Index</h1></body></html>","text/html");
        }
        public IActionResult Detail(string id)
        {
             return Content($"Sono Detail con id: {id}");
        }
    }
}