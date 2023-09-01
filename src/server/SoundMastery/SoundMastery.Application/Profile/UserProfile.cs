namespace SoundMastery.Application.Profile;

public class UserProfile
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
}