using DuolingoTechPlatform.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // RF28 — Relatórios / métricas da plataforma
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalCompletions = await _context.UserProgress.CountAsync();
            var today = DateTime.UtcNow.Date;
            var activeToday = await _context.Users
                .Where(u => u.LastActivityDate.HasValue && u.LastActivityDate.Value.Date == today)
                .CountAsync();

            var avgXp = totalUsers > 0
                ? await _context.Users.AverageAsync(u => (double)u.XP)
                : 0.0;

            var topUserByXp = await _context.Users
                .OrderByDescending(u => u.XP)
                .Select(u => new { u.Name, u.XP, u.Level })
                .FirstOrDefaultAsync();

            // Most completed course by counting UserProgress per course
            var mostCompletedCourse = await _context.UserProgress
                .Join(_context.Lessons, up => up.LessonId, l => l.Id, (up, l) => new { l.ModuleId })
                .Join(_context.Modules, x => x.ModuleId, m => m.Id, (x, m) => new { m.CourseId })
                .Join(_context.Courses, x => x.CourseId, c => c.Id, (x, c) => new { c.Id, c.Title })
                .GroupBy(x => new { x.Id, x.Title })
                .Select(g => new { g.Key.Title, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync();

            var totalCourses = await _context.Courses.CountAsync();
            var totalLessons = await _context.Lessons.CountAsync();
            var totalExercises = await _context.Exercises.CountAsync();

            return Ok(new
            {
                totalUsers,
                totalCompletions,
                activeToday,
                averageXp = Math.Round(avgXp, 1),
                topUser = topUserByXp,
                mostCompletedCourse = mostCompletedCourse?.Title ?? "N/A",
                totalCourses,
                totalLessons,
                totalExercises
            });
        }
    }
}
