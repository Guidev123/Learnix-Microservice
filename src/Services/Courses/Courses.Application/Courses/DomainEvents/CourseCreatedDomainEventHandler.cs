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
    internal sealed class CourseCreatedDomainEventHandler(
        ICourseRepository courseRepository,
         IMessageBus messageBus) : DomainEventHandler<CourseCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(CourseCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(domainEvent.CourseId, cancellationToken: cancellationToken)
                ?? throw new LearnixException(nameof(CourseCreatedDomainEvent), CourseErrors.NotFound(domainEvent.CourseId));

            await messageBus.ProduceAsync(Topics.CourseAttached, new CourseAttachedIntegrationEvent(
                    domainEvent.CorrelationId,
                    domainEvent.OccurredOn,
                    course.Id,
                    course.Specification.Title,
                    course.Specification.Description,
                    nameof(course.DificultLevel),
                    nameof(course.Status),
                    [.. course.Modules.Select(x => x.MapFromEntityToIntegrationEvent(course))]
                    ), cancellationToken);
        }
    }
}