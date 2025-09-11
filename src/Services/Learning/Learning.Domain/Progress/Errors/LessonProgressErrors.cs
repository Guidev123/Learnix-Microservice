using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class LessonProgressErrors
    {
        public static readonly Error LessonIdMustBeNotEmpty = Error.Failure(
            "LessonsProgress.LessonIdMustBeNotEmpty",
            "Lesson ID must be not empty");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Failure(
            "LessonsProgress.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error LessonProgressNotFound = Error.NotFound(
            "LessonsProgress.LessonProgressNotFound",
            "Lesson progress not found");

        public static readonly Error ModuleProgressIdMustBeNotEmpty = Error.Failure(
            "LessonsProgress.ModuleProgressIdMustBeNotEmpty",
            "Module Progress ID must be not empty");
    }
}