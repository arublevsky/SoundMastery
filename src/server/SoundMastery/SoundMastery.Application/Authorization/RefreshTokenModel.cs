namespace SoundMastery.Application.Authorization
{
    public class RefreshTokenModel
    {
        public string Username { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
