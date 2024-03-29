using System;

namespace pgm3.Models.Exceptions.Application
{
    public class CourseImageInvalidException : Exception
    {
        public CourseImageInvalidException(int courseId, Exception innerException) : base($"Image for course '{courseId}' is not valid", innerException)
        {
        }
    }
}