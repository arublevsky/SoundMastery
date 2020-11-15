using System;

namespace SoundMastery.Domain.Identity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; }
    }
}
