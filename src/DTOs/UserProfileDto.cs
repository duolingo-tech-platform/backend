using System;

namespace DuolingoTechPlatform.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Streak { get; set; }
        public bool ShowInRanking { get; set; }
        public string? Bio { get; set; }
    }
}