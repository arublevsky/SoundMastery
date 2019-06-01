using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SoundMastery.Domain.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public IList<User> Users { get; set; }
    }
}
