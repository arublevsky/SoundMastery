﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SoundMastery.Domain.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public virtual IList<Role> Roles { get; set; } = new List<Role>();

        public virtual IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
