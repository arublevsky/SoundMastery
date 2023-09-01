using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Identity;

/// <summary>
/// The wrapper around ASP.NET identity managers
/// </summary>
public interface IIdentityManager
{
    Task<SignInResult> PasswordSignInAsync(string username, string password);

    Task<IdentityResult> CreateAsync(User user, string password);
}