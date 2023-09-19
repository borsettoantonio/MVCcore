using System.ComponentModel.DataAnnotations;
//using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pgm3.Models.Services.Application.Courses;
using pgm3.Models.ViewModels.Courses;

namespace MyCourse.Pages;

//[ValidateReCaptcha]
public class ContactModel : PageModel
{
    public CourseDetailModel Course { get; private set; }

    [Required(ErrorMessage = "Il testo della domanda è obbligatorio")]
    [Display(Name = "La tua domanda")]
    [BindProperty]
    public string Question { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, [FromServices] ICourseService courseService)
    {
        try
        {
            Course = await courseService.GetCourseAsync(id);
            ViewData["Title"] = $"Invia una domanda";
            return Page();
        }
        catch
        {
            return RedirectToAction("Index", "Courses");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, [FromServices] ICourseService courseService, string prova)
    {
        if (ModelState.IsValid)
        {
            await courseService.SendQuestionToCourseAuthorAsync(id, Question);
            TempData["ConfirmationMessage"] = "La tua domanda è stata inviata";
            return RedirectToAction("Detail", "Courses", new { id = id });
        }
        else
        {
            return await OnGetAsync(id, courseService);
        }
    }
}