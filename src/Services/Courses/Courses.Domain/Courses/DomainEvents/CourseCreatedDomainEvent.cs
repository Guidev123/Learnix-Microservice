using Learnix.Commons.Domain.DomainEvents;

namespace Courses.Domain.Courses.DomainEvents
{
    public sealed record CourseCreatedDomainEvent : DomainEvent
    {
        private CourseCreatedDomainEvent()
        { }

        public CourseCreatedDomainEvent(Guid courseId)
        {
            CourseId = courseId;
            AggregateId = courseId;
            Messagetype = GetType().Name;
        }

        public Guid CourseId { get; init; }
    }
}