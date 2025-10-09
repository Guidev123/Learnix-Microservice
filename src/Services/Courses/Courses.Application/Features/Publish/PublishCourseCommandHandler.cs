using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using FluentValidation;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Features.Publish
{
    internal sealed class PublishCourseCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<PublishCourseCommand>
    {
        public async Task<Result> ExecuteAsync(PublishCourseCommand request, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(request.CourseId, cancellationToken: cancellationToken);
            if (course is null)
            {
                return Result.Failure(CourseErrors.NotFound(request.CourseId));
            }

            if (course.Modules.Count == 0)
            {
                return Result.Failure(CourseErrors.CanNotPublishCourseWithoutModules);
            }

            if (course.Modules.Where(m => m.Lessons.Count == 0).Any())
            {
                return Result.Failure(CourseErrors.CanNotPublishCourseWithModulesWithoutLessons);
            }

            course.Publish();
            courseRepository.Update(course);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success()
                : Result.Failure(CourseErrors.FailToPersistChanges(course.Id));
        }
    }
}