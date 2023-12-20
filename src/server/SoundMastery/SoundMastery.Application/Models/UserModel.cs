using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Models;

public class UserModel
{
    public UserModel()
    {
    }

    public UserModel(User user)
    {
        Id = user.Id;
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        UserName = user.UserName;
        RefreshTokens = user.RefreshTokens;
        Roles = user.Roles;
        Avatar = user.Avatar != null ? Convert.ToBase64String(user.Avatar) : null;
        IndividualLessons = user.IndividualLessons;
    }

    public int Id { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    public string Avatar { get; set; }

    public bool HasRole(string role) => Roles.Any(x => x.Name == role);

    [JsonIgnore]
    public IList<RefreshToken> RefreshTokens { get; set; }

    [JsonIgnore]
    public IList<Role> Roles { get; set; }

    [JsonIgnore]
    public IList<IndividualLesson> IndividualLessons { get; set; }
}