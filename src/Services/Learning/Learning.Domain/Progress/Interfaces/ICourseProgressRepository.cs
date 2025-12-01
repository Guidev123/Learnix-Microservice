using Learning.Domain.Progress.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Learning.Domain.Progress.Interfaces
{
    public interface ICourseProgressRepository : IRepository<CourseProgress>
    {
        Task<CourseProgress?> GetByStudentAndCourseIdAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default);

        Task<ModuleProgress?> GetModuleProgressByModuleIdAsync(Guid courseProgressId, Guid moduleId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default);

        void Insert(CourseProgress courseProgress);

        void Update(CourseProgress courseProgress);

        void InsertModuleProgress(ModuleProgress moduleProgress);

        void InsertLessonProgress(LessonProgress lessonProgress);
    }
}