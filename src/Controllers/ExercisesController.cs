using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.DTOs;
using DuolingoTechPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            var correctOption = await _context.ExerciseOptions
                .Where(o => o.ExerciseId == exercise.Id && o.IsCorrect)
                .FirstOrDefaultAsync();

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
                CorrectOptionId = correctOption?.Id ?? Guid.Empty,
                Xp = user.XP,
                Level = user.Level,
                Streak = user.Streak,
                Message = isCorrect ? "Resposta correta! +10 XP" : "Resposta incorreta."
            });
        }

        [Authorize]
        [HttpGet("by-lesson/{lessonId}")]
        public async Task<IActionResult> GetByLesson(Guid lessonId)
        {
            // RF09 — prerequisite check
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) return NotFound(new { message = "Lição não encontrada." });

            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId != null)
            {
                var userGuid = Guid.Parse(userId);
                var prereqIds = await _context.Lessons
                    .Where(l => l.ModuleId == lesson.ModuleId && l.Order < lesson.Order)
                    .Select(l => l.Id)
                    .ToListAsync();

                if (prereqIds.Count > 0)
                {
                    var completedCount = await _context.UserProgress
                        .Where(up => up.UserId == userGuid && prereqIds.Contains(up.LessonId))
                        .CountAsync();

                    if (completedCount < prereqIds.Count)
                        return StatusCode(403, new { message = "Complete as lições anteriores primeiro." });
                }
            }

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

                // RF13 — V/F detection at runtime
                var isVF = options.Count == 2 &&
                    options.Any(o => o.Text == "Verdadeiro") &&
                    options.Any(o => o.Text == "Falso");

                result.Add(new ExerciseWithOptionsDto
                {
                    Id = ex.Id,
                    Question = ex.Question,
                    Type = isVF ? "true_false" : "multiple_choice",
                    Options = options
                });
            }

            return Ok(result);
        }

        // RF22/RF23 — Revisão inteligente baseada em erros
        [Authorize]
        [HttpGet("revision")]
        public async Task<IActionResult> GetRevision()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var userGuid = Guid.Parse(userId);

            // Exercises where user has wrong answers but never a correct answer
            var incorrectIds = await _context.UserAnswers
                .Where(ua => ua.UserId == userGuid && !ua.IsCorrect)
                .Select(ua => ua.ExerciseId)
                .Distinct()
                .ToListAsync();

            var correctIds = await _context.UserAnswers
                .Where(ua => ua.UserId == userGuid && ua.IsCorrect)
                .Select(ua => ua.ExerciseId)
                .Distinct()
                .ToListAsync();

            var reviewIds = incorrectIds.Except(correctIds).Take(10).ToList();

            if (reviewIds.Count == 0)
                return Ok(new List<ExerciseWithOptionsDto>());

            var exercises = await _context.Exercises
                .Where(e => reviewIds.Contains(e.Id))
                .ToListAsync();

            var result = new List<ExerciseWithOptionsDto>();
            foreach (var ex in exercises)
            {
                var options = await _context.ExerciseOptions
                    .Where(o => o.ExerciseId == ex.Id)
                    .Select(o => new ExerciseOptionDto { Id = o.Id, Text = o.Text })
                    .ToListAsync();

                var isVF = options.Count == 2 &&
                    options.Any(o => o.Text == "Verdadeiro") &&
                    options.Any(o => o.Text == "Falso");

                result.Add(new ExerciseWithOptionsDto
                {
                    Id = ex.Id,
                    Question = ex.Question,
                    Type = isVF ? "true_false" : "multiple_choice",
                    Options = options
                });
            }

            return Ok(result);
        }

        // RF27 — CRUD completo

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Exercise updated)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(updated.Question))
                exercise.Question = updated.Question;
            if (!string.IsNullOrWhiteSpace(updated.CorrectAnswer))
                exercise.CorrectAnswer = updated.CorrectAnswer;
            await _context.SaveChangesAsync();
            return Ok(exercise);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            // RN09 — não apagar exercício com respostas de usuários
            var hasAnswers = await _context.UserAnswers.AnyAsync(ua => ua.ExerciseId == id);
            if (hasAnswers)
                return Conflict(new { message = "Não é possível excluir: existem respostas de usuários para este exercício." });

            var options = _context.ExerciseOptions.Where(o => o.ExerciseId == id);
            _context.ExerciseOptions.RemoveRange(options);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Exercício deletado." });
        }
    }
}
