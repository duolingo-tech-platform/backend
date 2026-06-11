using DuolingoTechPlatform.Data;
using DuolingoTechPlatform.DTOs;
using DuolingoTechPlatform.Helpers;
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
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Name) ||
                string.IsNullOrWhiteSpace(dto?.Email) ||
                string.IsNullOrWhiteSpace(dto?.Password))
                return BadRequest(new { message = "Preencha todos os campos." });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email já cadastrado." });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                XP = 0,
                Level = 0,
                Streak = 0
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtHelper.GenerateToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Xp = user.XP,
                    Level = user.Level,
                    Streak = user.Streak,
                    ShowInRanking = user.ShowInRanking,
                    Bio = user.Bio
                }
            };
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Credenciais inválidas." });

            var token = _jwtHelper.GenerateToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Xp = user.XP,
                    Level = user.Level,
                    Streak = user.Streak,
                    ShowInRanking = user.ShowInRanking,
                    Bio = user.Bio
                }
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return NotFound();
            var profile = new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Xp = user.XP,
                Level = user.Level,
                Streak = user.Streak,
                ShowInRanking = user.ShowInRanking,
                Bio = user.Bio
            };
            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Email)) user.Email = dto.Email;
            if (dto.Bio != null) user.Bio = dto.Bio;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            if (!PasswordHasher.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
                return BadRequest(new { message = "Senha atual incorreta." });

            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 6)
                return BadRequest(new { message = "Nova senha deve ter pelo menos 6 caracteres." });

            user.PasswordHash = PasswordHasher.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Senha alterada com sucesso." });
        }

        [Authorize]
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();
            var userGuid = Guid.Parse(userId);

            var user = await _context.Users.FindAsync(userGuid);
            if (user == null) return NotFound();

            var progress = _context.UserProgress.Where(up => up.UserId == userGuid);
            _context.UserProgress.RemoveRange(progress);

            var answers = _context.UserAnswers.Where(ua => ua.UserId == userGuid);
            _context.UserAnswers.RemoveRange(answers);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Conta deletada com sucesso." });
        }

        // RF03 ── Recuperação de senha ──────────────────────────────────────────

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Email))
                return BadRequest(new { message = "E-mail obrigatório." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Ok(new { message = "Se esse e-mail estiver cadastrado, você receberá o código." });

            var code = new Random().Next(1000, 9999).ToString();
            user.ResetCode = code;
            user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Código gerado.", code });
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Email) || string.IsNullOrWhiteSpace(dto?.Code))
                return BadRequest(new { message = "E-mail e código obrigatórios." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || user.ResetCode != dto.Code || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest(new { message = "Código inválido ou expirado." });

            return Ok(new { valid = true });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Email) || string.IsNullOrWhiteSpace(dto?.Code) || string.IsNullOrWhiteSpace(dto?.NewPassword))
                return BadRequest(new { message = "Todos os campos são obrigatórios." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || user.ResetCode != dto.Code || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest(new { message = "Código inválido ou expirado." });

            if (dto.NewPassword.Length < 6)
                return BadRequest(new { message = "Nova senha deve ter pelo menos 6 caracteres." });

            user.PasswordHash = PasswordHasher.HashPassword(dto.NewPassword);
            user.ResetCode = null;
            user.ResetCodeExpiry = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Senha redefinida com sucesso." });
        }

        // RF29 ── Push token ────────────────────────────────────────────────────

        [Authorize]
        [HttpPost("push-token")]
        public async Task<IActionResult> RegisterPushToken([FromBody] PushTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Token))
                return BadRequest(new { message = "Token obrigatório." });

            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            user.PushToken = dto.Token;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Token registrado." });
        }

        // RN08 ── Opt-out do ranking ───────────────────────────────────────────

        [Authorize]
        [HttpPut("ranking-visibility")]
        public async Task<IActionResult> SetRankingVisibility([FromBody] RankingVisibilityDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            user.ShowInRanking = dto.ShowInRanking;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Preferência atualizada.", showInRanking = user.ShowInRanking });
        }
    }
}
