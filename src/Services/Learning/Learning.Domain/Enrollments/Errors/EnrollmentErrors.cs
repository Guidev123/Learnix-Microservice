using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class EnrollmentErrors
    {
        public static Error NotFound(Guid studentId) => Error.NotFound(
            "Enrollments.NotFoundForEnrollmentId",
            $"Enrollment for student with ID {studentId} was not found");

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
            "CourseProgress ID must be not empty");

        public static readonly Error EnrollmentDateCannotBeInFuture = Error.Problem(
            "Enrollments.EnrollmentDateCannotBeInFuture",
            "Enrollment date cannot be in the future");

        public static readonly Error EndDateMustBeAfterEnrollmentDate = Error.Problem(
            "Enrollments.EndDateMustBeAfterEnrollmentDate",
            "End date must be after the enrollment date");

        public static readonly Error CourseMustBeNotNull = Error.Problem(
            "Enrollments.CourseMustBeNotNull",
            "CourseProgress must be not null");

        public static readonly Error CourseProgressStudentIdDoesNotMatchEnrollmentStudentId = Error.Problem(
            "Enrollments.CourseProgressStudentIdDoesNotMatchEnrollmentStudentId",
            "CourseProgress student ID does not match Enrollment student ID");

        public static readonly Error CourseProgressCourseIdDoesNotMatchEnrollmentCourseId = Error.Problem(
            "Enrollments.CourseProgressCourseIdDoesNotMatchEnrollmentCourseId",
            "CourseProgress course ID does not match Enrollment course ID");

        public static Error EnrollmentNotFoundForGivenStudentAndCourse(Guid studentId, Guid courseId) => Error.NotFound(
            "Enrollments.EnrollmentNotFoundForGivenStudentAndCourse",
            $"Enrollment not found for student with ID {studentId} and course with ID {courseId}");
    }
}