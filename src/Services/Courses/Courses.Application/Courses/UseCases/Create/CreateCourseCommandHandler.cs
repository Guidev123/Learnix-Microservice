using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Courses.UseCases.Create
{
    internal sealed class CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CreateCourseCommand, CreateCourseResponse>
    {
        public async Task<Result<CreateCourseResponse>> ExecuteAsync(CreateCourseCommand request, CancellationToken cancellationToken = default)
        {
            var category = await courseRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
            if (category is null)
            {
                return Result.Failure<CreateCourseResponse>(CategoryErrors.NotFound(request.CategoryId));
            }

            var course = Course.Create(
                request.Title,
                request.Description,
                request.DificultLevel,
                request.CategoryId);

            courseRepository.Insert(course);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? new CreateCourseResponse(course.Id)
                : Result.Failure<CreateCourseResponse>(CourseErrors.FailToPersist(course.Id));
        }
    }
}