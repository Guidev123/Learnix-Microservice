using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Enrollment : Entity, IAggregateRoot
    {
        public const int MaxEnrollmentDurationInDays = 365;

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
        public Course? Course { get; private set; }

        public static Enrollment Create(Guid studentId, Guid courseId, DateTime enrolledAt, DateTime endsAt)
        {
            var enrollment = new Enrollment(studentId, courseId, enrolledAt, endsAt);

            return enrollment;
        }

        public void StartCourse()
        {
            EnsureCourseIsNotNull();
            Course?.StartCourse();
        }

        public void CompleteCourse(DateTime completedAt)
        {
            EnsureCourseIsNotNull();
            Course?.CompleteCourse(completedAt);
        }

        public void StartModule(Guid moduleId)
        {
            EnsureCourseIsNotNull();
            Course?.StartModule(moduleId);
        }

        public void CompleteModule(Guid moduleId)
        {
            EnsureCourseIsNotNull();
            Course?.CompleteModule(moduleId);
        }

        public void CompleteLesson(Guid moduleId, Guid lessonId)
        {
            EnsureCourseIsNotNull();
            Course?.CompleteLesson(moduleId, lessonId);
        }

        private void EnsureCourseIsNotNull()
        {
            if (Course is null)
            {
                throw new DomainException(EnrollmentErrors.CourseMustBeNotNull.Description);
            }
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(StudentId, Guid.Empty, EnrollmentErrors.StudentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, EnrollmentErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(EnrolledAt <= DateTime.UtcNow, EnrollmentErrors.EnrollmentDateCannotBeInFuture.Description);
            AssertionConcern.EnsureTrue(EndsAt > EnrolledAt, EnrollmentErrors.EndDateMustBeAfterEnrollmentDate.Description);
            AssertionConcern.EnsureTrue(EndsAt <= EnrolledAt.AddDays(MaxEnrollmentDurationInDays), EnrollmentErrors.EndDateMustBeWithinMaxDuration(MaxEnrollmentDurationInDays).Description);
        }
    }
}