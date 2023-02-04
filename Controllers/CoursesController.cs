using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICourseService courseService)
        {
            this.courseService=courseService;
        }
        public async Task<IActionResult> Index()
        {
            List<CourseViewModel> courses= await courseService.GetCoursesAsync();
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
             CourseDetailModel viewModel=await courseService.GetCourseAsync(id);
            return View(viewModel);
        }
    }
}