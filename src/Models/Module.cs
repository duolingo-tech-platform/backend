using System;

namespace DuolingoTechPlatform.Models
{
    public class Module
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}