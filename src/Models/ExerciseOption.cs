using System;

namespace DuolingoTechPlatform.Models
{
    public class ExerciseOption
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}