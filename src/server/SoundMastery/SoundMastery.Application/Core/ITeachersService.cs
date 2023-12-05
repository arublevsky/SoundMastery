using System.Collections.Generic;
using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Core;

public interface ITeachersService
{
    Task<IReadOnlyCollection<UserModel>> GetList();

    Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId);
}