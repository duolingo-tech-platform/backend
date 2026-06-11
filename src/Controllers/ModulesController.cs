using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/modules")]
    public class ModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return NotFound();
            return Ok(module);
        }

        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetLessons(Guid id)
        {
            var exists = await _context.Modules.AnyAsync(m => m.Id == id);
            if (!exists) return NotFound();

            var lessons = await _context.Lessons
                .Where(l => l.ModuleId == id)
                .OrderBy(l => l.Order)
                .ToListAsync();

            return Ok(lessons);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Module module)
        {
            if (string.IsNullOrWhiteSpace(module.Title))
                return BadRequest(new { message = "Title is required." });
            if (module.CourseId == Guid.Empty)
                return BadRequest(new { message = "CourseId is required." });

            module.Id = Guid.NewGuid();
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = module.Id }, module);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Module updated)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(updated.Title)) module.Title = updated.Title;
            if (updated.Order > 0) module.Order = updated.Order;
            await _context.SaveChangesAsync();
            return Ok(module);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return NotFound();

            // RN09 — não apagar módulo cujas lições têm progresso
            var lessonIds = await _context.Lessons
                .Where(l => l.ModuleId == id)
                .Select(l => l.Id)
                .ToListAsync();

            if (lessonIds.Count > 0)
            {
                var hasProgress = await _context.UserProgress
                    .AnyAsync(up => lessonIds.Contains(up.LessonId));
                if (hasProgress)
                    return Conflict(new { message = "Não é possível excluir: existem usuários com progresso neste módulo." });
            }

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Módulo deletado." });
        }
    }
}