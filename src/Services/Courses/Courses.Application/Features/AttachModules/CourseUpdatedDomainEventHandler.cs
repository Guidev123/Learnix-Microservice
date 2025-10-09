using Courses.Application.Shared;
using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Courses.IntegrationEvents;

namespace Courses.Application.Features.AttachModules
{
    internal sealed class CourseUpdatedDomainEventHandler(
        ICourseRepository courseRepository,
        IMessageBus messageBus
        ) : DomainEventHandler<CourseUpdatedDomainEvent>
    {
        public override async Task ExecuteAsync(CourseUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(domainEvent.CourseId, cancellationToken: cancellationToken)
                ?? throw new LearnixException(nameof(CourseCreatedDomainEvent), CourseErrors.NotFound(domainEvent.CourseId));

            var lessons = course.Modules.SelectMany(m => m.Lessons).ToList();
            var lessonsResponse = lessons.Select(x => x.MapFromEntityToIntegrationEvent(course)).ToList();

            var integrationEvent = new CourseAttachedIntegrationEvent(
                    domainEvent.CorrelationId,
                    domainEvent.OccurredOn,
                    course.Id,
                    course.Specification.Title,
                    course.Specification.Description,
                    course.DificultLevel.ToString(),
                    course.Status.ToString(),
                    [.. course.Modules.Select(x => x.MapFromEntityToIntegrationEvent(course))],
                    lessonsResponse
                    );
            await messageBus.ProduceAsync(Topics.CourseAttached, integrationEvent, cancellationToken);
        }
    }
}