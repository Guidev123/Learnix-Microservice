using Learnix.Commons.Domain.Results;

namespace Learnix.Commons.Domain.ValueObjects
{
    public static class ValueObjectErrors
    {
        public static readonly Error FullNameMustBeNotEmpty = Error.Problem(
            "Name.FullNameMustBeNotEmpty",
            "Full name cannot be null or empty");

        public static readonly Error FullNameMustContainFirstNameAndLastName = Error.Problem(
            "Name.FullNameMustContainFirstNameAndLastName",
            "Full name must contain a first name and a last name");

        public static readonly Error EmailMustBeNotEmpty = Error.Problem(
            "Email.EmailMustBeNotEmpty",
            "Email address cannot be empty");

        public static readonly Error EmailOutOfRange = Error.Problem(
            "Email.EmailOutOfRange",
            $"E-mail length must be less or equal {Email.MaxEmailLength}");

        public static readonly Error EmailMustBeValid = Error.Problem(
            "Email.EmailMustBeValid",
            "Email address is not in a valid format");

        public static readonly Error FirstNameMustBeNotEmpty = Error.Problem(
            "Email.FirstNameMustBeNotEmpty",
            "First name cannot be empty");

        public static readonly Error FirstNameLengthInvalid = Error.Problem(
            "Name.FirstNameLengthInvalid",
            $"First name must be between {Name.NameMinLength} and {Name.NameMaxLength} characters");

        public static readonly Error LastNameMustBeNotEmpty = Error.Problem(
            "Name.LastNameMustBeNotEmpty",
            "Last name cannot be empty");

        public static readonly Error LastNameLengthInvalid = Error.Problem(
            "Name.LastNameLengthInvalid",
            $"Last name must be between {Name.NameMinLength} and {Name.NameMaxLength} characters");

        public static readonly Error BirthDateCannotBeInFuture = Error.Problem(
            "Age.BirthDateCannotBeInFuture",
            "Birth date cannot be in the future");

        public static readonly Error BirthDateCannotBeEmpty = Error.Problem(
            "Age.BirthDateCannotBeEmpty",
            "Birth date cannot be empty");

        public static readonly Error FullNameIsOutOfRange = Error.Problem(
            "Name.FullNameIsOutOfRange",
            $"First and Last name must be between {Name.NameMinLength} and {Name.NameMaxLength}");

        public static readonly Error AgeOutOfRange = Error.Problem(
            "Age.AgeOutOfRange",
            $"Age must be between {Age.MinAge} and {Age.MaxAge} years");
    }
}