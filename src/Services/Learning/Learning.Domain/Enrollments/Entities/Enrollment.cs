using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Students.Entities;
using Learning.Domain.Students.Errors;
using Learnix.Commons.Domain.DomainObjects;
using System.Reflection;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Enrollment : Entity, IAggregateRoot
    {
        public const int EnrollmentDurationInDays = 365;

        private Enrollment(Guid studentId, Guid courseId, DateTime enrolledAt, DateTime endsAt)
        {
            StudentId = studentId;
            CourseId = courseId;
            EnrolledAt = enrolledAt;
            EndsAt = endsAt;
            Status = EnrollmentStatusEnum.Active;
            Validate();
        }

        private Enrollment()
        { }

        public Guid StudentId { get; }
        public Guid CourseId { get; }
        public DateTime EnrolledAt { get; }
        public DateTime EndsAt { get; }
        public EnrollmentStatusEnum Status { get; private set; }

        public static Enrollment Create(Student student, Guid courseId, DateTime enrolledAt)
        {
            if (student.Subscription is null)
            {
                throw new DomainException(StudentErrors.ToEnrollYouNeedSubscription.Description);
            }

            var enrollment = new Enrollment(student.Id, courseId, enrolledAt, student.Subscription.ExpiresAt);

            return enrollment;
        }

        public static void StartCourse(CourseProgress courseProgress)
        {
            courseProgress.StartCourse();
        }

        public static void CompleteCourse(DateTime completedAt, CourseProgress courseProgress)
        {
            courseProgress.CompleteCourse(completedAt);
        }

        public static void StartModule(Guid moduleId, CourseProgress courseProgress)
        {
            courseProgress.StartModule(moduleId);
        }

        public void CompleteModule(Guid moduleId, CourseProgress courseProgress)
        {
            courseProgress.CompleteModule(moduleId, Id);
        }

        public void CompleteLesson(Guid moduleId, Guid lessonId, CourseProgress courseProgress)
        {
            courseProgress.CompleteLesson(moduleId, lessonId, Id);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(StudentId, Guid.Empty, EnrollmentErrors.StudentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, EnrollmentErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(EnrolledAt <= DateTime.UtcNow, EnrollmentErrors.EnrollmentDateCannotBeInFuture.Description);
            AssertionConcern.EnsureTrue(EndsAt > EnrolledAt, EnrollmentErrors.EndDateMustBeAfterEnrollmentDate.Description);
        }
    }
}