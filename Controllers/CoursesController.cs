using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.ViewModels;
using MyCourse.Models.InputModels;
using pgm3.Models.Services.Application.Courses;
using pgm3.Models.ViewModels.Courses;
using pgm3.Models.Exceptions.Application;
using MyCourse.Models.InputModels.Courses; 

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IHttpContextAccessor ctx;
        public CoursesController(ICachedCourseService courseService, IHttpContextAccessor ctx)
        {
            this.courseService = courseService;
            this.ctx = ctx;
        }

        [ResponseCache(CacheProfileName = "Home")]
        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            ViewData["Title"] = ctx.HttpContext.Request.Method;
            ListViewModel<CourseViewModel> courses = await courseService.GetCoursesAsync(input);
            ViewData["Title"] = "Catalogo dei corsi";
            CourseListViewModel viewModel = new CourseListViewModel
            {
                Courses = courses,
                Input = input
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int idCliente,int id)
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
                    TempData["ConfirmationMessage"] = "Ok! Il tuo corso è stato creato, ora perché non inserisci anche gli altri dati?";
                    return RedirectToAction(nameof(Edit), new { id = course.Id });
                }
                catch (CourseTitleUnavailableException)
                {
                    ModelState.AddModelError(nameof(CourseDetailModel.Title), "Questo titolo già esiste");
                }
            }
            return View(inputModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Modifica corso";
            CourseEditInputModel inputModel = await courseService.GetCourseForEditingAsync(id);
            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CourseEditInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                // sanitizzazione del contenuto HTML della textarea Descrizione
                inputModel.Sanitizza();
                try
                {
                    CourseDetailModel course = await courseService.EditCourseAsync(inputModel);
                    TempData["ConfirmationMessage"] = "I dati sono stati salvati con successo";
                    return RedirectToAction(nameof(Detail), new { id = inputModel.Id });
                }
              
                catch (CourseImageInvalidException)
                {
                    ModelState.AddModelError(nameof(CourseEditInputModel.Image), "L'immagine selezionata non è valida");
                }
          
                catch (CourseTitleUnavailableException)
                {
                    ModelState.AddModelError(nameof(CourseEditInputModel.Title), "Questo titolo già esiste");
                }
              
            }
            ViewData["Title"] = "Modifica corso";
            return View(inputModel);
        }
        public async Task<IActionResult> IsTitleAvailable(string title, int id = 0)
        {
            bool result = await courseService.IsTitleAvailableAsync(title, id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CourseDeleteInputModel inputModel)
        {
            await courseService.DeleteCourseAsync(inputModel);
            TempData["ConfirmationMessage"] = "Il corso è stato eliminato ma potrebbe continuare a comparire negli elenchi per un breve periodo, finché la cache non viene aggiornata.";
            return RedirectToAction(nameof(Index));
        }
    }
}