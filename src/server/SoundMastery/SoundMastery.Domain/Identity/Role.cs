using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SoundMastery.Domain.Identity;

public class Role : IdentityRole<int>
{
    public virtual IList<User> Users { get; set; } = new List<User>();
}