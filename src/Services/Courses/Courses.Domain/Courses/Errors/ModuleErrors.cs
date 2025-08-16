using Learnix.Commons.Domain.Results;

namespace Courses.Domain.Courses.Errors
{
    public static class ModuleErrors
    {
        public static Error NotFound(Guid moduleId)
            => Error.NotFound(
                "Modules.NotFound",
                $"The module with ID '{moduleId}' was not found.");

        public static readonly Error TitleMustBeNotEmpty = Error.Problem(
            "Modules.TitleMustBeNotEmpty",
            "The module title must not be empty");

        public static Error TitleMustBeInRange(int minLength, int maxLength)
            => Error.Problem(
                "Modules.TitleMustBeInRange",
                $"The module title must be between {minLength} and {maxLength} characters long");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
                "Modules.CourseIdMustBeNotEmpty",
                "Course ID must be not empty");

        public static readonly Error FailToPersistModules = Error.Problem(
            "Modules.FailToPersistModules",
            "Fail to persist modules");
    }
}