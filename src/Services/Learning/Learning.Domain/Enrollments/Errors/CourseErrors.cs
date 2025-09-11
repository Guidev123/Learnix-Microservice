using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class CourseErrors
    {
        public static Error NotFound(Guid courseId) => Error.NotFound
            ("Courses.NotFound",
            $"Course with ID '{courseId}' was not found");

        public static readonly Error CourseTitleMustBeProvided = Error.Failure
            ("Courses.CourseTitleMustBeProvided",
            "Course title must be provided");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Failure
            ("Courses.CourseIdMustBeNotEmpty",
            "Course ID must be not empty");

        public static readonly Error CourseDescriptionMustBeProvided = Error.Failure
            ("Courses.CourseDescriptionMustBeProvided",
            "Course description must be provided");
    }
}