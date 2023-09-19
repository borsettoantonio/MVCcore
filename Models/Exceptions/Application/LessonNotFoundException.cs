using System;

namespace pgm3.Models.Exceptions.Application
{
    public class LessonNotFoundException : Exception
    {
        public LessonNotFoundException(int lessonId) : base($"Lesson {lessonId} not found")
        {
        }
    }
}