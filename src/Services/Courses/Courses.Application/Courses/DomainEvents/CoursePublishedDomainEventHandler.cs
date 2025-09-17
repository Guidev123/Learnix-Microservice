using Courses.Application.Courses.Mappers;
using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Courses.IntegrationEvents;

namespace Courses.Application.Courses.DomainEvents
{
    internal sealed class CoursePublishedDomainEventHandler(
        ICourseRepository courseRepository,
        IMessageBus messageBus) : DomainEventHandler<CoursePublishedDomainEvent>
    {
        public override async Task ExecuteAsync(CoursePublishedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(domainEvent.CourseId, cancellationToken: cancellationToken)
                ?? throw new LearnixException(nameof(CourseCreatedDomainEvent), CourseErrors.NotFound(domainEvent.CourseId));

            var lessons = course.Modules.SelectMany(m => m.Lessons).ToList();

            await messageBus.ProduceAsync(Topics.CourseAttached, new CourseAttachedIntegrationEvent(
                    domainEvent.CorrelationId,
                    domainEvent.OccurredOn,
                    course.Id,
                    course.Specification.Title,
                    course.Specification.Description,
                    course.DificultLevel.ToString(),
                    course.Status.ToString(),
                    [.. course.Modules.Select(x => x.MapFromEntityToIntegrationEvent(course))],
                    [.. lessons.Select(x => x.MapFromEntityToIntegrationEvent(course))]
                    ), cancellationToken);
        }
    }
}