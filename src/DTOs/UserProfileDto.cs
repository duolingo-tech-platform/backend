using System;

namespace DuolingoTechPlatform.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Streak { get; set; }
    }
}