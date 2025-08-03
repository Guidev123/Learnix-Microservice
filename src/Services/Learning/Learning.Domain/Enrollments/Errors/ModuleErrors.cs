using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class ModuleErrors
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "Modules.NotFound",
            $"Module with ID '{id}' not found");

        public static readonly Error TotalLessonsMustBeGreaterThanZero = Error.Problem(
            "Modules.TotalLessonsMustBeGreaterThanZero",
            "Total lessons must be greater than zero");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
             "Modules.CourseIdMustBeNotEmpty",
             "Course ID must be not empty");

        public static readonly Error LessonsMustNotBeEmpty = Error.Problem(
            "Modules.LessonsMustNotBeEmpty",
            "Lessons must not be empty");
    }
}