using Learnix.Commons.Domain.Results;

namespace Courses.Domain.Courses.Errors
{
    public static class CourseErrors
    {
        public static Error NotFound(Guid courseId)
            => Error.NotFound(
                "Courses.NotFound",
                $"The course with ID '{courseId}' was not found.");

        public static readonly Error TitleMustBeNotEmpty = Error.Problem(
            "Courses.TitleMustBeNotEmpty",
            "The course title must not be empty.");

        public static readonly Error DescriptionMustBeNotEmpty = Error.Problem(
            "Courses.DescriptionMustBeNotEmpty",
            "The course description must not be empty.");

        public static Error TitleMustBeInRange(int minLength, int maxLength)
            => Error.Problem(
                "Courses.TitleMustBeInRange",
                $"The course title must be between {minLength} and {maxLength} characters long");

        public static Error DescriptionMustBeInRange(int minLength, int maxLength)
            => Error.Problem(
                "Courses.DescriptionMustBeInRange",
                $"The course description must be between {minLength} and {maxLength} characters long");

        public static readonly Error CourseSpecificationMustBeNotNull = Error.Problem(
            "Courses.CourseSpecificationMustBeNotNull",
            "The course specification must not be null");

        public static readonly Error DificultLevelMustBeValid = Error.Problem(
            "Courses.DificultLevelMustBeValid",
            "Dificult level must be valid");

        public static readonly Error CategoryIdMustBeNotEmpty = Error.Problem(
            "Courses.CategoryIdMustBeNotEmpty",
            "The category ID must not be empty");

        public static Error FailToPersist(Guid id) =>
            Error.Problem(
            "Courses.FailToPersist",
            $"Fail to persist course with ID {id}");
    }
}