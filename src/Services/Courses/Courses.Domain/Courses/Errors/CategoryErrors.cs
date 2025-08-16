using Learnix.Commons.Domain.Results;

namespace Courses.Domain.Courses.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error NameMustBeNotEmpty = Error.Problem(
            "Categories.NameMustBeNotEmpty",
            "Category name must be not empty");

        public static Error NameMustBeInRange(int minLength, int maxLength)
            => Error.Problem(
                "Categories.NameMustBeInRange",
                $"Category name must be between {minLength} and {maxLength} characters long.");

        public static Error NotFound(Guid id)
            => Error.NotFound(
                "Categories.NotFound",
                $"Category with ID {id} not found");
    }
}