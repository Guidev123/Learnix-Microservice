using Courses.Application.Courses.Abstractions;
using Courses.Application.Courses.Mappers;
using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.DomainEvents
{
    internal sealed class CourseCreatedDomainEventHandler(
        ICourseRepository courseRepository,
        ICourseContentRepository courseContentRepository) : DomainEventHandler<CourseCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(CourseCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesAndLessonsAsync(domainEvent.CourseId, cancellationToken: cancellationToken)
                ?? throw new LearnixException(nameof(CourseCreatedDomainEvent), CourseErrors.NotFound(domainEvent.CourseId));

            await courseContentRepository.InsertAsync(course.MapFromEntity(), cancellationToken);
        }
    }
}