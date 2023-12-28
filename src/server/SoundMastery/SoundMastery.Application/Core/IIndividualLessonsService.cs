using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Core;

public interface IIndividualLessonsService
{
    Task<IReadOnlyCollection<IndividualLessonModel>> GetMyIndividualLessons(int userId);

    Task<IReadOnlyCollection<IndividualLessonMaterialModel>> GetIndividualLessonMaterials(int userId, int lessonId);

    Task<bool> AddIndividualLesson(AddIndividualLessonModel model);

    Task<IndividualLessonsAvailabilityModel> GetAvailableLessons(int teacherId, DateTime date);

    Task<bool> CancelIndividualLesson(int userId, int lessonId);

    Task<bool> CompleteIndividualLesson(int userId, int lessonId);

    Task<bool> AddMaterial(AddMaterialModel model);
}