using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lesson lesson)
        {
            if (string.IsNullOrWhiteSpace(lesson.Title))
                return BadRequest(new { message = "Title is required." });
            if (lesson.ModuleId == Guid.Empty)
                return BadRequest(new { message = "ModuleId is required." });

            lesson.Id = Guid.NewGuid();
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, lesson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Lesson updated)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(updated.Title)) lesson.Title = updated.Title;
            if (!string.IsNullOrWhiteSpace(updated.Content)) lesson.Content = updated.Content;
            if (updated.Order > 0) lesson.Order = updated.Order;
            await _context.SaveChangesAsync();
            return Ok(lesson);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            // RN09 — não apagar lição com progresso de usuários
            var hasProgress = await _context.UserProgress.AnyAsync(up => up.LessonId == id);
            if (hasProgress)
                return Conflict(new { message = "Não é possível excluir: existem usuários com progresso nesta lição." });

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Lição deletada." });
        }
    }
}