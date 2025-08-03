using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class LessonErrors
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "Lessons.NotFound",
            $"Lesson with ID '{id}' not found");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "Lessons.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");
    }
}