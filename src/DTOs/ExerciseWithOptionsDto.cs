using System;
using System.Collections.Generic;

namespace DuolingoTechPlatform.DTOs
{
    public class ExerciseOptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }

    public class ExerciseWithOptionsDto
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public List<ExerciseOptionDto> Options { get; set; }
    }
}
