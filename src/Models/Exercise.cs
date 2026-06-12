using System;

namespace DuolingoTechPlatform.Models
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
    }
}