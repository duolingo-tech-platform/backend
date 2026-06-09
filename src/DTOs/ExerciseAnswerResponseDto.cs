namespace DuolingoTechPlatform.DTOs
{
    public class ExerciseAnswerResponseDto
    {
        public bool IsCorrect { get; set; }
        public Guid CorrectOptionId { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Streak { get; set; }
        public string Message { get; set; }
    }
}