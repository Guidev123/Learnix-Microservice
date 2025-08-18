using Courses.Application.Courses.UseCases.GetContent;

namespace Courses.Application.Courses.Abstractions
{
    public interface ICourseContentRepository
    {
        Task<CourseContentResponse> GetByIdAsync(Guid courseId, CancellationToken cancellationToken = default);

        Task InsertAsync(CourseContentResponse response, CancellationToken cancellationToken = default);

        Task ReplaceAsync(CourseContentResponse response, CancellationToken cancellationToken = default);
    }
}