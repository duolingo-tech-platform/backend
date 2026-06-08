using System;

namespace DuolingoTechPlatform.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Streak { get; set; }
        public DateTime? LastActivityDate { get; set; }
    }
}
