using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class EnrollmentErrors
    {
        public static readonly Error EnrollmentCouldNotBeSaved = Error.Problem(
            "Enrollments.EnrollmentCouldNotBeSaved",
            "Enrollment could not be saved");

        public static Error StudentAlreadyEnrolled(Guid studentId, Guid courseId) => Error.Problem(
            "Enrollments.StudentAlreadyEnrolled",
            $"Student with ID {studentId} is already enrolled in course with ID {courseId}");

        public static readonly Error StudentIdMustBeNotEmpty = Error.Problem(
            "Enrollments.StudentIdMustBeNotEmpty",
            "Student ID must be not empty");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
            "Enrollments.CourseIdMustBeNotEmpty",
            "Course ID must be not empty");

        public static readonly Error EnrollmentDateCannotBeInFuture = Error.Problem(
            "Enrollments.EnrollmentDateCannotBeInFuture",
            "Enrollment date cannot be in the future");

        public static readonly Error EndDateMustBeAfterEnrollmentDate = Error.Problem(
            "Enrollments.EndDateMustBeAfterEnrollmentDate",
            "End date must be after the enrollment date");

        public static Error EndDateMustBeWithinMaxDuration(int maxEnrollmentDurationInDays) => Error.Problem(
            "Enrollments.EndDateMustBeWithinMaxDuration",
            $"End date must be within {maxEnrollmentDurationInDays} days of enrollment date.");

        public static readonly Error CourseMustBeNotNull = Error.Problem(
            "Enrollments.CourseMustBeNotNull",
            "Course must be not null");
    }
}