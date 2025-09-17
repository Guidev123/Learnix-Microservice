using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class ModuleErrors
    {
        public static readonly Error ModuleIdMustBeNotEmpty = Error.Failure(
            "Modules.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error ModuleTitleMustBeProvided = Error.Failure(
            "Modules.ModuleTitleMustBeProvided",
            "Module title must be provided");

        public static Error NotFound(Guid id) => Error.NotFound(
            "Modules.NotFound",
            $"Module with ID {id} was not found");
    }
}