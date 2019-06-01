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
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        public virtual IList<Role> Roles { get; set; }
    }
}
