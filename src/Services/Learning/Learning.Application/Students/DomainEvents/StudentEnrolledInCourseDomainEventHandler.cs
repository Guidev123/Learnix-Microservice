using Learning.Domain.DomainEvents;
using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Students.DomainEvents
{
    internal sealed class StudentEnrolledInCourseDomainEventHandler : DomainEventHandler<StudentEnrolledInCourseDomainEvent>
    {
        public override Task ExecuteAsync(StudentEnrolledInCourseDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}