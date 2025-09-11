using Learning.Application.Enrollments.UseCases.AttachCourse;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Courses.IntegrationEvents;
using MidR.Interfaces;

namespace Learning.Infrastructure.Enrollments.IntegrationEvents
{
    internal sealed class CourseAttachedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<CourseAttachedIntegrationEvent>
    {
        public override async Task ExecuteAsync(CourseAttachedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await sender.SendAsync(new AttachCourseCommand(
                integrationEvent.CourseId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.DificultLevel,
                integrationEvent.Status,
                integrationEvent.Modules.Select(c => new ModuleRequest(
                    c.Id,
                    c.Title,
                    c.OrderIndex,
                    c.Lessons.Select(c => new LessonRequest(
                        c.Id,
                        c.Title,
                        c.VideoUrl,
                        c.OrderIndex,
                        c.ModuleId,
                        c.NextLessonId,
                        c.PreviousLessonId
                        )).ToList(),
                    c.CourseId,
                    c.NextModuleId,
                    c.PreviousModuleId)).ToList()), cancellationToken);

            if (result.IsFailure)
            {
                throw new LearnixException(nameof(AttachCourseCommand), result.Error);
            }
        }
    }
}