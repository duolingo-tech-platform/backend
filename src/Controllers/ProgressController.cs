using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
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
                .Include(up => up.LessonId)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userGuid);

            return Ok(new
            {
                xp = user?.XP ?? 0,
                level = user?.Level ?? 0,
                streak = user?.Streak ?? 0,
                lessonsCompleted = progress.Count,
                lessons = progress.Select(up => up.LessonId)
            });
        }
    }
}
