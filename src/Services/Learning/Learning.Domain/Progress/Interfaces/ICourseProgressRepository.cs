using Learning.Domain.Progress.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Learning.Domain.Progress.Interfaces
{
    public interface ICourseProgressRepository : IRepository<CourseProgress>
    {
        Task<CourseProgress> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(CourseProgress courseProgress);
    }
}