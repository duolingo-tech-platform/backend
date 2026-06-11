using DuolingoTechPlatform.Data;
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
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourseProgress()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var userGuid = Guid.Parse(userId);

            var completedLessonIdList = await _context.UserProgress
                .Where(up => up.UserId == userGuid)
                .Select(up => up.LessonId)
                .ToListAsync();
            var completedLessonIds = completedLessonIdList.ToHashSet();

            var courses = await _context.Courses.ToListAsync();

            var result = new List<object>();
            foreach (var course in courses)
            {
                var moduleIds = await _context.Modules
                    .Where(m => m.CourseId == course.Id)
                    .Select(m => m.Id)
                    .ToListAsync();

                var totalLessons = await _context.Lessons
                    .Where(l => moduleIds.Contains(l.ModuleId))
                    .CountAsync();

                var completedLessons = await _context.Lessons
                    .Where(l => moduleIds.Contains(l.ModuleId) && completedLessonIds.Contains(l.Id))
                    .CountAsync();

                result.Add(new
                {
                    courseId = course.Id,
                    completed = completedLessons,
                    total = totalLessons,
                    percent = totalLessons > 0 ? (int)Math.Round((double)completedLessons / totalLessons * 100) : 0
                });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var userGuid = Guid.Parse(userId);

            var progressList = await _context.UserProgress
                .Where(up => up.UserId == userGuid)
                .OrderByDescending(up => up.CompletedAt)
                .ToListAsync();

            if (!progressList.Any())
                return Ok(new List<object>());

            var lessonIds = progressList.Select(up => up.LessonId).ToList();

            var lessons = await _context.Lessons
                .Where(l => lessonIds.Contains(l.Id))
                .ToDictionaryAsync(l => l.Id);

            var moduleIds = lessons.Values.Select(l => l.ModuleId).Distinct().ToList();
            var modules = await _context.Modules
                .Where(m => moduleIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id);

            var courseIds = modules.Values.Select(m => m.CourseId).Distinct().ToList();
            var courses = await _context.Courses
                .Where(c => courseIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var exercisesPerLesson = await _context.Exercises
                .Where(e => lessonIds.Contains(e.LessonId))
                .GroupBy(e => e.LessonId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(e => e.Id).ToList());

            var allExerciseIds = exercisesPerLesson.Values.SelectMany(x => x).ToList();

            var correctSet = allExerciseIds.Any()
                ? (await _context.UserAnswers
                    .Where(ua => ua.UserId == userGuid && allExerciseIds.Contains(ua.ExerciseId) && ua.IsCorrect)
                    .Select(ua => ua.ExerciseId)
                    .ToListAsync()).ToHashSet()
                : new HashSet<Guid>();

            var result = progressList
                .Where(up => lessons.ContainsKey(up.LessonId))
                .Select(up =>
                {
                    var lesson = lessons[up.LessonId];
                    modules.TryGetValue(lesson.ModuleId, out var module);
                    var course = module != null && courses.ContainsKey(module.CourseId) ? courses[module.CourseId] : null;
                    var exIds = exercisesPerLesson.GetValueOrDefault(lesson.Id) ?? new List<Guid>();
                    var correct = exIds.Count(id => correctSet.Contains(id));
                    return new
                    {
                        lessonId = lesson.Id,
                        lessonTitle = lesson.Title,
                        moduleTitle = module != null ? module.Title : "",
                        courseTitle = course != null ? course.Title : "",
                        xpEarned = correct * 10,
                        totalExercises = exIds.Count,
                        correctAnswers = correct,
                        completedAt = up.CompletedAt
                    };
                })
                .ToList();

            return Ok(result);
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
