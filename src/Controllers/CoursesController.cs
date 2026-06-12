using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Title))
                return BadRequest(new { message = "Title is required." });

            course.Id = Guid.NewGuid();
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpGet("{id}/modules")]
        public async Task<IActionResult> GetModules(Guid id)
        {
            var exists = await _context.Courses.AnyAsync(c => c.Id == id);
            if (!exists) return NotFound();

            var modules = await _context.Modules
                .Where(m => m.CourseId == id)
                .OrderBy(m => m.Order)
                .ToListAsync();

            return Ok(modules);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Course updated)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(updated.Title)) course.Title = updated.Title;
            if (!string.IsNullOrWhiteSpace(updated.Description)) course.Description = updated.Description;
            await _context.SaveChangesAsync();
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            // RN09 — não apagar curso com progresso de usuários
            var moduleIds = await _context.Modules
                .Where(m => m.CourseId == id)
                .Select(m => m.Id)
                .ToListAsync();

            if (moduleIds.Count > 0)
            {
                var lessonIds = await _context.Lessons
                    .Where(l => moduleIds.Contains(l.ModuleId))
                    .Select(l => l.Id)
                    .ToListAsync();

                if (lessonIds.Count > 0)
                {
                    var hasProgress = await _context.UserProgress
                        .AnyAsync(up => lessonIds.Contains(up.LessonId));
                    if (hasProgress)
                        return Conflict(new { message = "Não é possível excluir: existem usuários com progresso neste curso." });
                }
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Curso deletado." });
        }
    }
}