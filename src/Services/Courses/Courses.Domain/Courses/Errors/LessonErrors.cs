using Learnix.Commons.Domain.Results;

namespace Courses.Domain.Courses.Errors
{
    public static class LessonErrors
    {
        public static Error NotFound(Guid lessonId)
            => Error.NotFound(
                "Lessons.NotFound",
                $"The lesson with ID '{lessonId}' was not found.");

        public static Error UrlMustBeNotEmpty
            => Error.Problem(
                "Lessons.UrlMustBeNotEmpty",
                "The video URL must not be empty.");

        public static Error ThumbnailUrlMustBeNotEmpty
            => Error.Problem(
                "Lessons.ThumbnailUrlMustBeNotEmpty",
                "The thumbnail URL must not be empty.");

        public static Error UrlMustBeValid
            => Error.Problem(
                "Lessons.UrlMustBeValid",
                "The video URL must be a valid URL format.");

        public static Error ThumbnailUrlMustBeValid
            => Error.Problem(
                "Lessons.ThumbnailUrlMustBeValid",
                "The thumbnail URL must be a valid URL format.");

        public static readonly Error TitleMustBeNotEmpty = Error.Problem(
            "Lessons.TitleMustBeNotEmpty",
            "The lesson title must not be empty");

        public static Error TitleMustBeInRange(int minLength, int maxLength)
            => Error.Problem(
                "Lessons.TitleMustBeInRange",
                $"The lesson title must be between {minLength} and {maxLength} characters long");

        public static readonly Error ModuleIdMustBeNotEmpty = Error.Problem(
            "Lessons.ModuleIdMustBeNotEmpty",
            "The module ID must not be empty");

        public static readonly Error DurationInMinutesShouldBeGreaterThanZero = Error.Problem(
            "Lessons.DurationInMinutesShouldBeGreaterThanZero",
            "Duration in minutes should be greater than zero");

        public static readonly Error FailToPersistLessons = Error.Problem(
            "Lessons.FailToPersistLessons",
            "Fail to persist lessons");
    }
}