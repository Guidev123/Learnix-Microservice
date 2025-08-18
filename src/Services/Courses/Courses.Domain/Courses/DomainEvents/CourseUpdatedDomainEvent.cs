using Learnix.Commons.Domain.DomainEvents;

namespace Courses.Domain.Courses.DomainEvents
{
    public sealed record CourseUpdatedDomainEvent : DomainEvent
    {
        private CourseUpdatedDomainEvent()
        { }

        public CourseUpdatedDomainEvent(Guid courseId)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        public Guid CourseId { get; init; }
    }
}