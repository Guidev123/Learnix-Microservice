using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Students.DomainEvents;
using Learning.Domain.Students.Errors;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Learning.Domain.Students.Entities
{
    public sealed class Student : Entity, IAggregateRoot
    {
        private Student(Guid id, string firstName, string lastName, string email)
        {
            Id = id;
            Name = (firstName, lastName);
            Email = email;
            Validate();
        }

        private Student()
        { }

        public Email Email { get; private set; } = null!;
        public Name Name { get; private set; } = null!;

        private readonly List<Enrollment> _enrollments = [];
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

        public static Student Create(Guid id, string firstName, string lastName, string email)
        {
            var student = new Student(id, firstName, lastName, email);

            return student;
        }

        public void Enroll(Enrollment enrollment)
        {
            _enrollments.Add(enrollment);
            AddDomainEvent(new StudentEnrolledInCourseDomainEvent(Id, enrollment.Id));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Email, StudentErrors.EmailMustBeNotNull.Description);
            AssertionConcern.EnsureNotNull(Name, StudentErrors.NameMustBeNotNull.Description);
            AssertionConcern.EnsureDifferent(Guid.Empty, Id, StudentErrors.IdMustBeNotEmpty.Description);
        }
    }
}