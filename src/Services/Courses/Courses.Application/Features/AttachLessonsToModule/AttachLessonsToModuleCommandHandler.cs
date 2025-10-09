using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Enumerators;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Features.AttachLessonsToModule
{
    internal sealed class AttachLessonsToModuleCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AttachLessonsToModuleCommand>
    {
        public async Task<Result> ExecuteAsync(AttachLessonsToModuleCommand request, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(request.CourseId, false, cancellationToken);
            if (course is null)
            {
                return Result.Failure(ModuleErrors.NotFound(request.ModuleId));
            }

            if (course.Status == CourseStatusEnum.Published)
            {
                return Result.Failure(CourseErrors.StatusMustBeNotPublished(request.CourseId));
            }

            var lessons = request.Lessons.Select(lesson => Lesson.Create(
                lesson.Title,
                lesson.VideoUrl,
                lesson.VideoThumbnailUrl,
                (uint)lesson.DurationInMinutes,
                request.ModuleId)
            );

            var module = course.GetModuleById(request.ModuleId);
            if (module is null)
            {
                return Result.Failure(ModuleErrors.NotFound(request.ModuleId));
            }

            course.AddLessonsToModule(module.Id, lessons);

            courseRepository.InsertLessonsToModuleRange(module.Lessons);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success()
                : Result.Failure(LessonErrors.FailToPersistLessons);
        }
    }
}