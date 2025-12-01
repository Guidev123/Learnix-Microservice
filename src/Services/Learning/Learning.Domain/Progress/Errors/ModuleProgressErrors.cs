using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class ModuleProgressErrors
    {
        public static Error NotFound(Guid moduleId) => Error.NotFound(
            "ModulesProgress.NotFound",
            $"Module progress with Module ID '{moduleId}' was not found");

        public static readonly Error ModuleProgressNotFound = Error.NotFound(
            "ModulesProgress.ModuleProgressNotFound",
            "Module progress not found");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "ModulesProgress.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error CourseProgressIdMustBeNotEmpty = Error.Problem(
            "ModulesProgress.CourseProgressIdMustBeNotEmpty",
            "Course Progress ID must be not empty");

        public static readonly Error FailToPersistModuleProgress = Error.Problem(
            "ModulesProgress.FailToPersistModuleProgress",
            "Fail to persist module progress");
    }
}