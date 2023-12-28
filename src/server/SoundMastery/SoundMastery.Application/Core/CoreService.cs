using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;
using static SoundMastery.Application.Constants;

namespace SoundMastery.Application.Core;

public class CoreService : ICoreService
{
    private readonly IGenericRepository<IndividualLesson> _lessonsRepository;
    private readonly IGenericRepository<User> _userRepository;

    public CoreService(
        IGenericRepository<IndividualLesson> lessonsRepository,
        IGenericRepository<User> userRepository)
    {
        _lessonsRepository = lessonsRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyCollection<UserModel>> GetTeachers()
    {
        var users = await _userRepository.Find(x => x.Roles.Any(role => role.Name.Equals(Roles.Teacher)));
        return users.Select(x => new UserModel(x)).ToArray();
    }

    public async Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId)
    {
        var lessons = await _lessonsRepository.Find(x => x.StudentId == userId);
        return lessons.Select(x => new UserModel(x.Teacher)).ToArray();
    }
}