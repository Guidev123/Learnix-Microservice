using Learnix.Commons.Domain.DomainEvents;

namespace Courses.Domain.Courses.DomainEvents
{
    public sealed record CoursePublishedDomainEvent : DomainEvent
    {
        private CoursePublishedDomainEvent()
        { }

        public CoursePublishedDomainEvent(Guid courseId)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        public Guid CourseId { get; init; }
    }
}