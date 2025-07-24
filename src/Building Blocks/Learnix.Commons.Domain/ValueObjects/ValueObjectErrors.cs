using Learnix.Commons.Domain.Results;

namespace Learnix.Commons.Domain.ValueObjects
{
    public static class ValueObjectErrors
    {
        public static readonly Error FullNameMustBeNotEmpty = Error.Problem(
            "ValueObjects.FullNameMustBeNotEmpty",
            "Full name cannot be null or empty");

        public static readonly Error FullNameMustContainFirstNameAndLastName = Error.Problem(
            "ValueObjects.FullNameMustContainFirstNameAndLastName",
            "Full name must contain a first name and a last name");

        public static readonly Error EmailMustBeNotEmpty = Error.Problem(
            "ValueObjects.EmailMustBeNotEmpty",
            "Email address cannot be empty");

        public static readonly Error EmailMustBeValid = Error.Problem(
            "ValueObjects.EmailMustBeValid",
            "Email address is not in a valid format");

        public static readonly Error FirstNameMustBeNotEmpty = Error.Problem(
            "ValueObjects.FirstNameMustBeNotEmpty",
            "First name cannot be empty");

        public static readonly Error FirstNameLengthInvalid = Error.Problem(
            "ValueObjects.FirstNameLengthInvalid",
            $"First name must be between {Name.NameMinLength} and {Name.NameMaxLength} characters");

        public static readonly Error LastNameMustBeNotEmpty = Error.Problem(
            "ValueObjects.LastNameMustBeNotEmpty",
            "Last name cannot be empty");

        public static readonly Error LastNameLengthInvalid = Error.Problem(
            "ValueObjects.LastNameLengthInvalid",
            $"Last name must be between {Name.NameMinLength} and {Name.NameMaxLength} characters");

        public static readonly Error BirthDateCannotBeInFuture = Error.Problem(
            "ValueObjects.BirthDateCannotBeInFuture",
            "Birth date cannot be in the future");

        public static readonly Error AgeOutOfRange = Error.Problem(
            "ValueObjects.AgeOutOfRange",
            $"Age must be between {Age.MinAge} and {Age.MaxAge} years");
    }
}