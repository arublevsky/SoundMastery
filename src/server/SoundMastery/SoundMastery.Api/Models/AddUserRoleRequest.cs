using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Api.Models;

public class AddUserRoleRequest
{
    [Required]
    public int? UserId { get; set; }

    [Required]
    public string Name { get; set; }
}