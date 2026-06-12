using System;

namespace DuolingoTechPlatform.Models
{
    public class UserAnswer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ExerciseId { get; set; }
        public Guid SelectedOptionId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }
    }
}