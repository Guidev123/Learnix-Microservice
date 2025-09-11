using Learning.Domain.Progress.Entities;
using Learning.Domain.Progress.Interfaces;

namespace Learning.Infrastructure.Progress.Repositories
{
    internal sealed class CourseProgressRepository : ICourseProgressRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<CourseProgress> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(CourseProgress courseProgress)
        {
            throw new NotImplementedException();
        }
    }
}