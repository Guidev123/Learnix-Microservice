using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record ModuleCompletedDomainEvent : DomainEvent
    {
        private ModuleCompletedDomainEvent()
        { }

        public ModuleCompletedDomainEvent(Guid moduleId, Guid courseId, Guid enrollmentId)
        {
            AggregateId = enrollmentId;
            ModuleId = moduleId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        public Guid ModuleId { get; init; }
        public Guid CourseId { get; init; }
    }
}