using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Core;

public class IndividualLessonsService : IIndividualLessonsService
{
    private readonly IGenericRepository<IndividualLesson> _lessonsRepository;
    private readonly IGenericRepository<User> _userRepository;

    public IndividualLessonsService(
        IGenericRepository<User> userRepository,
        IGenericRepository<IndividualLesson> lessonsRepository)
    {
        _userRepository = userRepository;
        _lessonsRepository = lessonsRepository;
    }

    public async Task<IReadOnlyCollection<IndividualLessonModel>> GetMyIndividualLessons(int userId)
    {
        var user = await _userRepository.Get(userId);
        var lessons = await FetchLessonsByRole(userId, user);
        return lessons.Select(lesson => new IndividualLessonModel(lesson)).ToList();
    }

    public async Task<IReadOnlyCollection<IndividualLessonMaterialModel>> GetIndividualLessonMaterials(int userId, int lessonId)
    {
        var lesson = await _lessonsRepository.Get(x => x.Id == lessonId);

        return lesson.TeacherId == userId || lesson.StudentId == userId
            ? lesson.Materials.Select(x => new IndividualLessonMaterialModel(x)).ToArray()
            : Enumerable.Empty<IndividualLessonMaterialModel>().ToArray();
    }

    public async Task<bool> AddIndividualLesson(AddIndividualLessonModel model)
    {
        var user = await _userRepository.Get(model.TeacherId);
        if (!user.HasRole(Constants.Roles.Teacher))
        {
            Log.Warning($"User {model.TeacherId} is not a teacher.");
            return false;
        }

        if (user.WorkingHours == null || !user.WorkingHours.IsWorking(model.Time))
        {
            Log.Warning($"User {model.TeacherId} is not working at {model.Time}.");
            return false;
        }

        if (user.IndividualLessons.Any(x => x.Time == model.Time && x.Date == model.Date))
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
        var user = await _userRepository.Get(teacherId);

        var bookedHours = user.IndividualLessons
            .Where(x => x.Date == date.Date)
            .Select(x => x.Time.Hours);

        return new IndividualLessonsAvailabilityModel
        {
            AvailableHours = user.WorkingHours?.GetAvailableHours(bookedHours) ?? Array.Empty<int>()
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

    public Task<bool> AddMaterial(AddMaterialModel model)
    {
        return UpdateIndividualLesson(
            model.UserId,
            model.LessonId,
            lesson =>
            {
                lesson.Materials.Add(new IndividualLessonMaterial
                {
                    Description = model.Description,
                    Material = new Material
                    {
                        Type = model.FileId.HasValue ? MaterialType.File : MaterialType.Link,
                        FileId = model.FileId,
                        Url = model.Url
                    }
                });
            });
    }

    private Task<IReadOnlyCollection<IndividualLesson>> FetchLessonsByRole(int userId, User user)
    {
        if (user.HasRole(Constants.Roles.Teacher))
        {
            return _lessonsRepository.Find(x => x.TeacherId == userId);
        }

        if (user.HasRole(Constants.Roles.Student))
        {
            return _lessonsRepository.Find(x => x.StudentId == userId);
        }

        Log.Error($"Requesting lessons user is not a teacher or student: {userId}");
        return Task.FromResult((IReadOnlyCollection<IndividualLesson>)Array.Empty<IndividualLesson>());
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