using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Courses.UseCases.AttachLessonsToModule
{
    internal sealed class AttachLessonsToModuleCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AttachLessonsToModuleCommand>
    {
        public async Task<Result> ExecuteAsync(AttachLessonsToModuleCommand request, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesByIdAsync(request.CourseId, cancellationToken);
            if (course is null)
            {
                return Result.Failure(ModuleErrors.NotFound(request.ModuleId));
            }

            var lessons = request.Lessons.Select(lesson => Lesson.Create(
                lesson.Title,
                lesson.VideoUrl,
                lesson.VideoThumbnailUrl,
                (uint)lesson.DurationInMinutes,
                request.ModuleId)
            );

            AddLessonsToModule(lessons, course, request.ModuleId);

            courseRepository.InsertLessonsToModuleRange(lessons);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success()
                : Result.Failure(LessonErrors.FailToPersistLessons);
        }

        private static void AddLessonsToModule(
            IEnumerable<Lesson> lessons,
            Course course,
            Guid moduleId
            )
        {
            foreach (var lesson in lessons)
            {
                course.AddLessonToModule(moduleId, lesson);
            }
        }
    }
}