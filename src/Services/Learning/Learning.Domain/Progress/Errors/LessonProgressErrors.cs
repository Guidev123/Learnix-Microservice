using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class LessonProgressErrors
    {
        public static readonly Error FailToPersistChanges = Error.Problem(
            "LessonsProgress.FailToPersistChanges",
            "Fail to persist lesson progress changes");

        public static readonly Error LessonIdMustBeNotEmpty = Error.Problem(
            "LessonsProgress.LessonIdMustBeNotEmpty",
            "Lesson ID must be not empty");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "LessonsProgress.ModuleIdMustBeNotEmpty",
            "Module ID must be not empty");

        public static readonly Error LessonProgressNotFound = Error.NotFound(
            "LessonsProgress.LessonProgressNotFound",
            "Lesson progress not found");

        public static readonly Error ModuleProgressIdMustBeNotEmpty = Error.Problem(
            "LessonsProgress.ModuleProgressIdMustBeNotEmpty",
            "Module Progress ID must be not empty");

        public static readonly Error FailToPersist = Error.Problem(
            "LessonProgress.FailToPersist",
            "Fail to persist Lesson Progress");
    }
}