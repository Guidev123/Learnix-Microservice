using Learning.Application.Features.AttachCourse;
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
            var command = new AttachCourseCommand(
                    integrationEvent.CourseId,
                    integrationEvent.Title,
                    integrationEvent.Description,
                    integrationEvent.DificultLevel,
                    integrationEvent.Status,
                    integrationEvent.Modules.Select(m =>
                        new ModuleRequest(
                            m.Id,
                            m.Title,
                            m.OrderIndex,
                            integrationEvent.Lessons
                                .Where(l => l.ModuleId == m.Id)
                                .Select(l => new LessonRequest(
                                    l.Id,
                                    l.Title,
                                    l.VideoUrl,
                                    l.OrderIndex,
                                    l.ModuleId,
                                    l.NextLessonId,
                                    l.PreviousLessonId
                                ))
                                .ToList(),
                            m.CourseId,
                            m.NextModuleId,
                            m.PreviousModuleId
                        )
                    ).ToList()
                );
            var result = await sender.SendAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                throw new LearnixException(nameof(AttachCourseCommand), result.Error);
            }
        }
    }
}