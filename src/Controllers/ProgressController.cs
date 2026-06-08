using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProgressController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProgress()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var userGuid = Guid.Parse(userId);

            var progress = await _context.UserProgress
                .Where(up => up.UserId == userGuid)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userGuid);

            return Ok(new
            {
                xp = user?.XP ?? 0,
                level = user?.Level ?? 0,
                streak = user?.Streak ?? 0,
                lessonsCompleted = progress.Count,
                lessonIds = progress.Select(up => up.LessonId)
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserProgress userProgress)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            if (userProgress.LessonId == Guid.Empty)
                return BadRequest(new { message = "LessonId is required." });

            var userGuid = Guid.Parse(userId);

            var alreadyCompleted = await _context.UserProgress
                .AnyAsync(up => up.UserId == userGuid && up.LessonId == userProgress.LessonId);

            if (!alreadyCompleted)
            {
                userProgress.Id = Guid.NewGuid();
                userProgress.UserId = userGuid;
                userProgress.CompletedAt = DateTime.UtcNow;
                _context.UserProgress.Add(userProgress);
            }

            var user = await _context.Users.FindAsync(userGuid);
            if (user != null)
            {
                var today = DateTime.UtcNow.Date;
                if (user.LastActivityDate == null || user.LastActivityDate.Value.Date < today)
                {
                    var yesterday = today.AddDays(-1);
                    if (user.LastActivityDate?.Date == yesterday)
                        user.Streak += 1;
                    else
                        user.Streak = 1;

                    user.LastActivityDate = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Progresso registrado.", streak = user?.Streak ?? 0 });
        }
    }
}
