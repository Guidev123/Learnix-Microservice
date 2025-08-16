using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class ModuleProgressErrors
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "ModuleProgress.NotFound",
            $"ModuleProgress with ID '{id}' not found");

        public static readonly Error TotalLessonsMustBeGreaterThanZero = Error.Problem(
            "ModuleProgress.TotalLessonsMustBeGreaterThanZero",
            "Total lessons must be greater than zero");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "ModuleProgress.ModuleIdMustBeNotEmpty",
            "ModuleProgress ID must be not empty");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
             "ModuleProgress.CourseIdMustBeNotEmpty",
             "CourseProgress ID must be not empty");

        public static readonly Error LessonsMustNotBeEmpty = Error.Problem(
            "ModuleProgress.LessonsMustNotBeEmpty",
            "Lessons must not be empty");
    }
}