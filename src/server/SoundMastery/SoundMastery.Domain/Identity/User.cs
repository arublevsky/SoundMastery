using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Core;

namespace SoundMastery.Domain.Identity;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public virtual IList<Course> Courses { get; set; } = new List<Course>();

    public virtual IList<Role> Roles { get; set; } = new List<Role>();

    public virtual IList<FollowingStudent> FollowingStudents { get; set; } = new List<FollowingStudent>();

    public virtual IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}