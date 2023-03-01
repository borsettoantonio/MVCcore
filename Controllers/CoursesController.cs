using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;
using MyCourse.Models.InputModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService=courseService;
        }

        [ResponseCache(CacheProfileName ="Home")]
        public async Task<IActionResult> Index(CourseListInputModel model)
        {
            List<CourseViewModel> courses= await courseService.GetCoursesAsync(model);
            ViewData["Title"] = "Catalogo dei corsi";
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
             CourseDetailModel viewModel=await courseService.GetCourseAsync(id);
            return View(viewModel);
        }
    }
}