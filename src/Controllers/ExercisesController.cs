using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.DTOs;
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
    [Route("api/exercises")]
    public class ExercisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("answer")]
        public async Task<IActionResult> Answer([FromBody] ExerciseAnswerDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return Unauthorized();

            var exercise = await _context.Exercises.FindAsync(dto.ExerciseId);
            if (exercise == null) return NotFound(new { message = "Exercício não encontrado." });

            var selectedOption = await _context.ExerciseOptions.FindAsync(dto.SelectedOptionId);
            if (selectedOption == null || selectedOption.ExerciseId != exercise.Id)
                return BadRequest(new { message = "Opção inválida." });

            bool isCorrect = selectedOption.IsCorrect;
            int xpEarned = isCorrect ? 10 : 0;
            if (isCorrect)
            {
                user.XP += xpEarned;
                user.Level = user.XP / 100;
            }

            _context.UserAnswers.Add(new UserAnswer
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ExerciseId = exercise.Id,
                SelectedOptionId = selectedOption.Id,
                IsCorrect = isCorrect,
                AnsweredAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok(new ExerciseAnswerResponseDto
            {
                IsCorrect = isCorrect,
                XP = user.XP,
                Level = user.Level,
                Streak = user.Streak,
                Message = isCorrect ? "Resposta correta! +10 XP" : "Resposta incorreta."
            });
        }

        [Authorize]
        [HttpGet("by-lesson/{lessonId}")]
        public async Task<IActionResult> GetByLesson(Guid lessonId)
        {
            var exercises = await _context.Exercises
                .Where(e => e.LessonId == lessonId)
                .ToListAsync();

            var result = new List<ExerciseWithOptionsDto>();
            foreach (var ex in exercises)
            {
                var options = await _context.ExerciseOptions
                    .Where(o => o.ExerciseId == ex.Id)
                    .Select(o => new ExerciseOptionDto { Id = o.Id, Text = o.Text })
                    .ToListAsync();

                result.Add(new ExerciseWithOptionsDto
                {
                    Id = ex.Id,
                    Question = ex.Question,
                    Options = options
                });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Exercise exercise)
        {
            if (string.IsNullOrWhiteSpace(exercise.Question))
                return BadRequest(new { message = "Question is required." });
            if (exercise.LessonId == Guid.Empty)
                return BadRequest(new { message = "LessonId is required." });

            exercise.Id = Guid.NewGuid();
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByLesson), new { lessonId = exercise.LessonId }, exercise);
        }
    }
}
