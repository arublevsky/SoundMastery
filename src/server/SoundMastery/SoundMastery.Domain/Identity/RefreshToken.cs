using System;

namespace SoundMastery.Domain.Identity;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}