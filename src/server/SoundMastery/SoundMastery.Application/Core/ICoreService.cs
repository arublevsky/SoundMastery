using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Core;

public interface ICoreService
{
    Task<IReadOnlyCollection<UserModel>> GetTeachers();

    Task<IReadOnlyCollection<UserModel>> GetMyTeachers(int userId);

    Task<IReadOnlyCollection<IndividualLessonModel>> GetMyIndividualLessons(int userId);

    Task<bool> AddIndividualLesson(AddIndividualLessonModel model);

    Task<IndividualLessonsAvailabilityModel> GetAvailableLessons(int teacherId, DateTime date);

    Task<bool> CancelIndividualLesson(int userId, int lessonId);

    Task<bool> CompleteIndividualLesson(int userId, int lessonId);

    Task<bool> AddMaterial(AddMaterialModel model);
}