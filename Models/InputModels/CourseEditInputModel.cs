using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AngleSharp.Html.Dom;
using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Controllers;

using MyCourse.Models.Enums;
using MyCourse.Models.ValueObjects;

namespace MyCourse.Models.InputModels
{
    public class CourseEditInputModel : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio"),
         MinLength(10, ErrorMessage = "Il titolo dev'essere di almeno {1} caratteri"),
         MaxLength(100, ErrorMessage = "Il titolo dev'essere di al massimo {1} caratteri"),
         RegularExpression(@"^[\w\s\.']+$", ErrorMessage = "Titolo non valido"),
         Remote(action: nameof(CoursesController.IsTitleAvailable), controller: "Courses", ErrorMessage = "Il titolo esiste già", AdditionalFields = "Id"),
         Display(Name = "Titolo")]
        public string Title { get; set; }

        [MinLength(21, ErrorMessage = "La descrizione dev'essere di almeno 10 caratteri"),
         MaxLength(4000, ErrorMessage = "La descrizione dev'essere di massimo {1} caratteri"),
         Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Display(Name = "Immagine rappresentativa")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "L'email di contatto è obbligatoria"),
         EmailAddress(ErrorMessage = "Devi inserire un indirizzo email"),
         Display(Name = "Email di contatto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il prezzo intero è obbligatorio"),
         Display(Name = "Prezzo intero")]
        public Money FullPrice { get; set; }

        [Required(ErrorMessage = "Il prezzo corrente è obbligatorio"),
         Display(Name = "Prezzo corrente")]
        public Money CurrentPrice { get; set; }

        [Display(Name = "Nuova immagine...  ")]
        public IFormFile Image { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FullPrice.Currency != CurrentPrice.Currency)
            {
                yield return new ValidationResult("Il prezzo intero deve avere la stessa valuta del prezzo corrente", new[] { nameof(FullPrice) });
            }
            else if (FullPrice.Amount < CurrentPrice.Amount)
            {
                yield return new ValidationResult("Il prezzo intero non può essere inferiore al prezzo corrente", new[] { nameof(FullPrice) });
            }
        }

        public static CourseEditInputModel FromDataRow(DataRow courseRow)
        {
            var courseEditInputModel = new CourseEditInputModel
            {
                Title = Convert.ToString(courseRow["Title"]),
                Description = Convert.ToString(courseRow["Description"]),
                ImagePath = Convert.ToString(courseRow["ImagePath"]),
                Email = Convert.ToString(courseRow["Email"]),
                FullPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                CurrentPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(courseRow["Id"])
            };
            return courseEditInputModel;
        }

        // metodo per controllare l'HTML della textarea Description
        public void Sanitizza()
        {
            var sanitizer = CreateSanitizer();
            Description = sanitizer.Sanitize(Description);
        }
        private static HtmlSanitizer CreateSanitizer()
        {
            var sanitizer = new HtmlSanitizer();

            //Tag consentiti
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("br");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("li");
            sanitizer.AllowedTags.Add("span");
            sanitizer.AllowedTags.Add("iframe");

            //Attributi consentiti
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.Add("src");
            sanitizer.AllowDataAttributes = false;

            //Stili consentiti
            sanitizer.AllowedCssProperties.Clear();

            sanitizer.FilterUrl += FilterUrl;
            sanitizer.PostProcessNode += ProcessIFrames;

            return sanitizer;
        }
        private static void FilterUrl(object sender, FilterUrlEventArgs filterUrlEventArgs)
        {
            if (!filterUrlEventArgs.OriginalUrl.StartsWith("//www.youtube.com/") && !filterUrlEventArgs.OriginalUrl.StartsWith("https://www.youtube.com/"))
            {
                filterUrlEventArgs.SanitizedUrl = null;
            }
        }
        
        private static void ProcessIFrames(object sender, PostProcessNodeEventArgs postProcessNodeEventArgs)
        {
            var iframe = postProcessNodeEventArgs.Node as IHtmlInlineFrameElement;
            if (iframe == null) {
                return;
            }
            var container = postProcessNodeEventArgs.Document.CreateElement("span");
            container.ClassName = "video-container";
            container.AppendChild(iframe.Clone(true));
            postProcessNodeEventArgs.ReplacementNodes.Add(container);
        }
    }
}