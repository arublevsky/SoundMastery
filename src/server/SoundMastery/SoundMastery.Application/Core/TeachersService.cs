using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.Application.Profile;

namespace SoundMastery.Application.Core;

public class TeachersService : ITeachersService
{
    private readonly IUserService _userService;

    public TeachersService(IUserService userService)
    {
        _userService = userService;
    }

    public Task<IReadOnlyCollection<UserModel>> GetList()
    {
        return _userService.Find(x => x.Roles.Any(role => role.Name.Equals("teacher")));
    }

    public Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId)
    {
        return _userService.Find(x =>
            x.Roles.Any(role => role.Name.Equals("teacher")) &&
            x.IndividualLessons.Any(lesson => lesson.StudentId == userId));
    }
}