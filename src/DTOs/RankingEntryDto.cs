using System;

namespace DuolingoTechPlatform.DTOs
{
    public class RankingEntryDto
    {
        public int Position { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Streak { get; set; }
    }
}
