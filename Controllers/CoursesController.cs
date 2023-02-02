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
        public IActionResult Index()
        {
            var courseService=new CourseService();
            List<CourseViewModel> courses= courseService.GetCourses();
            return View(courses);
        }
        public IActionResult Detail(int id)
        {
             var courseService=new CourseService();
             CourseDetailModel viewModel=courseService.GetCourse(id);
            return View(viewModel);
        }
    }
}