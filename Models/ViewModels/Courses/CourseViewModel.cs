using System.Data;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueObjects;

namespace pgm3.Models.ViewModels.Courses
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ImagePath { get; set; }
        public string? Author { get; set; }

        public double Rating { get; set; }
        public Money? FullPrice { get; set; }
        public Money? CurrentPrice { get; set; }
        public string Status { get; set; } = null!;

        /*
        public static CourseViewModel FromDataRow(DataRow course)
        {
            return new CourseViewModel
            {
                Id = Convert.ToInt32(course["Id"]),
                Title = (string)course["Title"],
                ImagePath = (string)course["ImagePath"],
                Author = (string)course["Author"],
                Rating = (double)course["Rating"],
                CurrentPrice = new Money(
                  Enum.Parse<Currency>(Convert.ToString(course["CurrentPrice_Currency"])),
                  Convert.ToDecimal(course["CurrentPrice_Amount"])),
                FullPrice = new Money(
                  Enum.Parse<Currency>(Convert.ToString(course["FullPrice_Currency"])),
                  Convert.ToDecimal(course["FullPrice_Amount"])),
                Status = (string)course["Status"],  
            };
        }
        */

    }
}