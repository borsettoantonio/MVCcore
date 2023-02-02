using System;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueObjects;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class CourseService
    {
        public List<CourseViewModel> GetCourses()
        {
            List<CourseViewModel> courseList = new();
            Random rand = new();
            for (int i = 1; i <= 20; i++)
            {
                decimal price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
                CourseViewModel course = new()
                {
                    Id = i,
                    Title = $"Corso {i}",
                    CurrentPrice = new Money(Currency.EUR, price),
                    FullPrice = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price - 1),
                    Author = "Nome cognome",
                    Rating = rand.Next(10, 50) / 10.0,
                    ImagePath = "/logo.svg"
                };
                courseList.Add(course);
            }
            return courseList;
        }

        public CourseDetailModel GetCourse(int id)
        {
            Random rand = new();
            decimal price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
            CourseDetailModel course = new()
            {
                Id = id,
                Title = $"Corso {id}",
                CurrentPrice = new Money(Currency.EUR, price),
                FullPrice = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price - 1),
                Author = "Nome cognome",
                Rating = rand.Next(10, 50) / 10.0,
                ImagePath = "/logo.svg",
                Description = $"Descrizione {id}",
                Lessons = new List<LessonViewModel>()
            };

            for (var i = 1; i <= 5; i++)
            {
                LessonViewModel lesson = new()
                {
                    Title = $"Lezione {i}",
                    Duration = TimeSpan.FromSeconds(rand.Next(40, 90))
                };
                course.Lessons.Add(lesson);
            }

            return course;

        }
    }
}