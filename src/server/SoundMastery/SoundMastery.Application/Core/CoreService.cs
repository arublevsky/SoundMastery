using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.Application.Profile;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Core;

public class CoreService : ICoreService
{
    private readonly IUserService _userService;
    private readonly IGenericRepository<IndividualLesson> _lessonsRepository;

    public CoreService(IUserService userService, IGenericRepository<IndividualLesson> lessonsRepository)
    {
        _userService = userService;
        _lessonsRepository = lessonsRepository;
    }

    public Task<IReadOnlyCollection<UserModel>> GetTeachers()
    {
        return _userService.Find(x => x.Roles.Any(role => role.Name.Equals("teacher")));
    }

    public Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId)
    {
        return _userService.Find(x =>
            x.Roles.Any(role => role.Name.Equals("teacher")) &&
            x.IndividualLessons.Any(lesson => lesson.StudentId == userId));
    }

    public async Task<IReadOnlyCollection<IndividualLessonModel>> GetMyIndividualLessons(int userId)
    {
        var lessons = await _lessonsRepository.Find(x => x.StudentId == userId);
        return lessons.Select(lesson => new IndividualLessonModel(lesson)).ToList();
    }

    public async Task<bool> AddIndividualLesson(AddIndividualLessonModel model)
    {
        var users = await _userService.Find(x =>
            x.Id == model.TeacherId && x.Roles.Any(role => role.Name.Equals("teacher")));

        if (!users.Any())
        {
            return false;
        }

        var result = await _lessonsRepository.Create(model.ToEntity());
        return result != null;
    }
}