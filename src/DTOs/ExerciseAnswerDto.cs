using System;

namespace DuolingoTechPlatform.DTOs
{
    public class ExerciseAnswerDto
    {
        public Guid ExerciseId { get; set; }
        public Guid SelectedOptionId { get; set; }
    }
}