using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

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
        public async Task<IActionResult> Index(string search,int page,string orderby,bool ascending)
        {
            List<CourseViewModel> courses= await courseService.GetCoursesAsync(search,page,orderby, ascending);
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