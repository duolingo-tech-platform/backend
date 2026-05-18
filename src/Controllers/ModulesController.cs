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
    }
}