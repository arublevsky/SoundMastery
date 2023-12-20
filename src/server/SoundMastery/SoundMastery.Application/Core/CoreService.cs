using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
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

    public async Task<IReadOnlyCollection<IndividualLessonModel>> GetMyIndividualLessons(int userId)
    {
        var user = await _userRepository.Get(userId);

        var lessons = await FetchLessonsByRole(userId, user);
        return lessons.Select(lesson => new IndividualLessonModel(lesson)).ToList();
    }

    private Task<IReadOnlyCollection<IndividualLesson>> FetchLessonsByRole(int userId, User user)
    {
        if (user.HasRole(Roles.Teacher))
        {
            return _lessonsRepository.Find(x => x.TeacherId == userId);
        }

        if (user.HasRole(Roles.Student))
        {
            return _lessonsRepository.Find(x => x.StudentId == userId);
        }

        Log.Error($"Requesting lessons user is not a teacher or student: {userId}");
        return Task.FromResult((IReadOnlyCollection<IndividualLesson>)Array.Empty<IndividualLesson>());
    }

    public async Task<bool> AddIndividualLesson(AddIndividualLessonModel model)
    {
        if (model.Date == DateTime.MinValue || !WorkingHours.Hours.Contains(model.Time.Hours))
        {
            Log.Warning($"Invalid lesson date or time: {model.Date} - {model.Time}");
            return false;
        }

        var users = await _userRepository.Find(x =>
            x.Id == model.TeacherId && x.Roles.Any(role => role.Name.Equals(Roles.Teacher)));

        if (!users.Any())
        {
            Log.Warning($"Could not found a teacher with ID: {model.TeacherId}");
            return false;
        }

        var teacher = users.Single();
        if (teacher.IndividualLessons.Any(x => x.Time == model.Time && x.Date == model.Date))
        {
            Log.Warning(
                $"Lesson for teacher ({model.TeacherId}) has been already booked at {model.Date} - {model.Time}.");
            return false;
        }

        var result = await _lessonsRepository.Create(model.ToEntity());
        return result != null;
    }

    public async Task<IndividualLessonsAvailabilityModel> GetAvailableLessons(int teacherId, DateTime date)
    {
        var lessons = await _lessonsRepository.Find(x => x.TeacherId == teacherId && x.Date == date.Date);
        var bookedHours = lessons.Select(x => TimeOnly.FromTimeSpan(x.Time).Hour);

        return new IndividualLessonsAvailabilityModel
        {
            AvailableHours = WorkingHours.Hours.Except(bookedHours).ToArray()
        };
    }

    public Task<bool> CancelIndividualLesson(int userId, int lessonId)
    {
        return UpdateIndividualLesson(
            userId,
            lessonId,
            lesson => lesson.Cancelled = true);
    }

    public Task<bool> CompleteIndividualLesson(int userId, int lessonId)
    {
        return UpdateIndividualLesson(
            userId,
            lessonId,
            lesson => lesson.Completed = true);
    }

    private async Task<bool> UpdateIndividualLesson(int userId, int lessonId, Action<IndividualLesson> updater)
    {
        var lesson = await _lessonsRepository.Get(lessonId);
        if (lesson.StudentId != userId && lesson.TeacherId != userId)
        {
            Log.Warning($"User {userId} is not allowed to update this lesson {lesson}.");
            return false;
        }

        updater(lesson);
        await _lessonsRepository.Update(lesson);
        return true;

    }
}