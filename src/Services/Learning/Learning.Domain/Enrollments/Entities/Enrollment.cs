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

        public static Enrollment Create(Guid studentId, Guid courseId, DateTime enrolledAt, DateTime endsAt)
        {
            var enrollment = new Enrollment(studentId, courseId, enrolledAt, endsAt);

            return enrollment;
        }

        public static void StartCourse(Course course)
        {
            course.StartCourse();
        }

        public static void CompleteCourse(DateTime completedAt, Course course)
        {
            course.CompleteCourse(completedAt);
        }

        public static void StartModule(Guid moduleId, Course course)
        {
            course.StartModule(moduleId);
        }

        public static void CompleteModule(Guid moduleId, Course course)
        {
            course.CompleteModule(moduleId);
        }

        public static void CompleteLesson(Guid moduleId, Guid lessonId, Course course)
        {
            course.CompleteLesson(moduleId, lessonId);
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