using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SoundMastery.Domain.Identity
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; } = string.Empty;

        public virtual IList<Role> Roles { get; set; } = new List<Role>();
    }
}
