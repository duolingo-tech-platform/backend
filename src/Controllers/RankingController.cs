using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/ranking")]
    public class RankingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRanking([FromQuery] int top = 10)
        {
            var users = await _context.Users
                .Where(u => u.ShowInRanking)
                .OrderByDescending(u => u.XP)
                .Take(top)
                .ToListAsync();

            var ranking = users.Select((u, index) => new RankingEntryDto
            {
                Position = index + 1,
                UserId = u.Id,
                Name = u.Name,
                Xp = u.XP,
                Level = u.Level,
                Streak = u.Streak
            }).ToList();

            return Ok(ranking);
        }
    }
}
