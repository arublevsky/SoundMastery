using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services;

public interface ISeedDataService
{
    Task ApplySeeds(User[] users, Role[] roles);
}