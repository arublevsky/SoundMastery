using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Core;

namespace SoundMastery.Domain.Identity;

public class User : IdentityUser<int>, IHasId
{
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    public byte[] Avatar { get; set; }

    public virtual IList<IndividualLesson> IndividualLessons { get; set; } = new List<IndividualLesson>();

    public virtual IList<Role> Roles { get; set; } = new List<Role>();

    public virtual IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public bool HasRole(string role) => Roles.Any(x => x.Name == role);
}