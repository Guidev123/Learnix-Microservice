using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class CourseErrors
    {
        public static Error NotFound(Guid courseId) => Error.NotFound
            ("Courses.NotFoundForEnrollmentId",
            $"Course with ID '{courseId}' was not found");

        public static readonly Error CourseTitleMustBeProvided = Error.Problem
            ("Courses.CourseTitleMustBeProvided",
            "Course title must be provided");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem
            ("Courses.CourseIdMustBeNotEmpty",
            "Course ID must be not empty");

        public static readonly Error CourseDescriptionMustBeProvided = Error.Problem
            ("Courses.CourseDescriptionMustBeProvided",
            "Course description must be provided");
    }
}