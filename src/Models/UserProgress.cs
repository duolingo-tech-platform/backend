using System;

namespace DuolingoTechPlatform.Models
{
    public class UserProgress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}