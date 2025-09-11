using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class ModuleErrors
    {
        public static readonly Error ModuleIdMustBeNotEmpty = Error.Failure(
            "Module.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error ModuleTitleMustBeProvided = Error.Failure(
            "Module.ModuleTitleMustBeProvided",
            "Module title must be provided");
    }
}