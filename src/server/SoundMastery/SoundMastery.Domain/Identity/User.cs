﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Core;

namespace SoundMastery.Domain.Identity;

public class User : IdentityUser<int>, IHasId
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public virtual IList<IndividualLesson> IndividualLessons { get; set; } = new List<IndividualLesson>();

    public virtual IList<Role> Roles { get; set; } = new List<Role>();

    public virtual IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public bool HasRole(string role) => Roles.Any(x => x.Name == role);
}