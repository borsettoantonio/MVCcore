using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;
using MyCourse.Models.InputModels;
using MyCourse.Models.Exceptions.Application;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;
        }

        [ResponseCache(CacheProfileName = "Home")]
        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            ListViewModel<CourseViewModel> courses = await courseService.GetCoursesAsync(input);
            ViewData["Title"] = "Catalogo dei corsi";
            CourseListViewModel viewModel = new CourseListViewModel
            {
                Courses = courses,
                Input = input
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailModel viewModel = await courseService.GetCourseAsync(id);
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Nuovo corso";
            var inputModel = new CourseCreateInputModel();
            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInputModel inputModel)
        {
            ViewData["Title"] = "Nuovo corso";
            if (ModelState.IsValid)
            {
                try
                {
                    CourseDetailModel course = await courseService.CreateCourseAsysnc(inputModel);
                    return RedirectToAction(nameof(Index));
                }
                catch (CourseTitleUnavailableException)
                {
                    ModelState.AddModelError(nameof(CourseDetailModel.Title), "Questo titolo già esiste");
                }
            }
            return View(inputModel);
        }
    }
}