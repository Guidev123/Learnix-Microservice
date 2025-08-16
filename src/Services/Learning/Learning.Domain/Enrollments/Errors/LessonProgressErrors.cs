using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class LessonProgressErrors
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "LessonProgress.NotFound",
            $"LessonProgress with ID '{id}' not found");

        public static readonly Error ModuleIdMustBeValid = Error.Problem(
            "LessonProgress.ModuleIdMustBeValid",
            "ModuleProgress ID must be valid");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "LessonProgress.ModuleIdMustBeNotEmpty",
            "ModuleProgress ID must be not empty");
    }
}