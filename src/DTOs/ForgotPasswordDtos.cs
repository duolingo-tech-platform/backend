namespace DuolingoTechPlatform.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class VerifyResetCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }

    public class PushTokenDto
    {
        public string Token { get; set; }
    }

    public class RankingVisibilityDto
    {
        public bool ShowInRanking { get; set; }
    }

    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
    }
}
