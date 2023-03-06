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
        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            ListViewModel<CourseViewModel> courses= await courseService.GetCoursesAsync(input);
            ViewData["Title"] = "Catalogo dei corsi";
            CourseListViewModel viewModel= new CourseListViewModel
            {
                Courses=courses,
                Input=input
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
             CourseDetailModel viewModel=await courseService.GetCourseAsync(id);
            return View(viewModel);
        }
    }
}