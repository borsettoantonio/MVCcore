using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Controllers;
using Models.InputModels;

namespace MyCourse.Models.InputModels
{
    public class CourseCreateInputModel
    {
        [Required(ErrorMessage ="Il titolo è obbligatorio"),
        MinLength(4,ErrorMessage ="Il titolo dev'essere almeno di {1} caratteri"),
        MaxLength(100,ErrorMessage ="Il titolo dev'essere al massimo di {1} caratteri"),
        RegularExpression(@"^[\w\s.]+$",ErrorMessage ="Titolo non valido"),
        NotPippo(ErrorMessage="Non deve essere Pippo")]
        public string? Title { get; set; }
    }
}