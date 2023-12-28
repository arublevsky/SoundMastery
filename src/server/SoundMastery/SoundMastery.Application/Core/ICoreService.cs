using System.Collections.Generic;
using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Core;

public interface ICoreService
{
    Task<IReadOnlyCollection<UserModel>> GetTeachers();

    Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId);
}