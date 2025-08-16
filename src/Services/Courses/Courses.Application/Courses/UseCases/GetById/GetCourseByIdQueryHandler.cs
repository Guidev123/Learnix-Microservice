using Courses.Application.Courses.Mappers;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Courses.UseCases.GetById
{
    internal sealed class GetCourseByIdQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCourseByIdQuery, GetCourseByIdResponse>
    {
        public async Task<Result<GetCourseByIdResponse>> ExecuteAsync(GetCourseByIdQuery request, CancellationToken cancellationToken = default)
        {
            var result = await courseRepository.GetWithModulesAndLessonsAsync(request.CourseId, cancellationToken);
            if (result is null)
            {
                return Result.Failure<GetCourseByIdResponse>(CourseErrors.NotFound(request.CourseId));
            }

            return result.MapFromEntity();
        }
    }
}