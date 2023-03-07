using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    // Specifica dei valori per l'header della risposta HTTP
    // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    // oppure per tutte le action del controller
    // [ResponseCache(CacheProfileName ="Home")]
    public class HomeController : Controller
    {
        [ResponseCache(CacheProfileName ="Home")]
        public async Task<IActionResult> Index([FromServices] ICachedCourseService courseService)
        {
            ViewData["Title"] = "Benvenuto su MyCourse!";
            List<CourseViewModel> bestRatingCourses = await courseService.GetBestRatingCoursesAsync();
            List<CourseViewModel> mostRecentCourses = await courseService.GetMostRecentCoursesAsync();

             HomeViewModel viewModel = new HomeViewModel
            {
                BestRatingCourses = bestRatingCourses,
                MostRecentCourses = mostRecentCourses
            };

            return View(viewModel);
        }
    }
}