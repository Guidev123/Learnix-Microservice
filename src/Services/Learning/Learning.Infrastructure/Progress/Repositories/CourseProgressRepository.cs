using Learning.Domain.Progress.Entities;
using Learning.Domain.Progress.Interfaces;
using Learning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Progress.Repositories
{
    internal sealed class CourseProgressRepository(LearningDbContext context) : ICourseProgressRepository
    {
        public Task<bool> ExistsAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default)
            => context.CoursesProgress.AsNoTracking().AnyAsync(cp => cp.StudentId == studentId && cp.CourseId == courseId, cancellationToken);

        public Task<CourseProgress?> GetByStudentAndCourseIdAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default)
            => context.CoursesProgress.Include(c => c.ModulesProgress).ThenInclude(m => m.LessonsProgress)
                .FirstOrDefaultAsync(c => c.StudentId == studentId && c.CourseId == courseId, cancellationToken);

        public Task<ModuleProgress?> GetModuleProgressByModuleIdAsync(Guid courseProgressId, Guid moduleId, CancellationToken cancellationToken = default)
            => context.ModulesProgress.AsNoTracking().FirstOrDefaultAsync(mp => mp.ModuleId == moduleId && mp.CourseProgressId == courseProgressId, cancellationToken);

        public void Insert(CourseProgress courseProgress)
            => context.CoursesProgress.Add(courseProgress);

        public void Update(CourseProgress courseProgress)
            => context.CoursesProgress.Update(courseProgress);

        public void InsertModuleProgress(ModuleProgress moduleProgress)
            => context.ModulesProgress.Add(moduleProgress);

        public void InsertLessonProgress(LessonProgress lessonProgress)
            => context.LessonsProgress.Add(lessonProgress);
    }
}