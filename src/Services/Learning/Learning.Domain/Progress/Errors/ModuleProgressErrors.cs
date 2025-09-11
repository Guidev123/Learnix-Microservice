using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class ModuleProgressErrors
    {
        public static readonly Error ModuleProgressNotFound = Error.NotFound(
            "ModulesProgress.ModuleProgressNotFound",
            "Module progress not found");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Failure(
            "ModulesProgress.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error CourseProgressIdMustBeNotEmpty = Error.Failure(
            "ModulesProgress.CourseProgressIdMustBeNotEmpty",
            "Course Progress ID must be not empty");
    }
}