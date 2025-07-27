using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Errors
{
    public static class StudentErrors
    {
        public static readonly Error FirstNameMustBeNotEmpty = Error.Problem(
            "Students.FirstNameMustBeNotEmpty",
            "First Name must be not empty");

        public static readonly Error LastNameMustBeNotEmpty = Error.Problem(
            "Students.LastNameMustBeNotEmpty",
            "Last Name must be not empty");

        public static readonly Error EmailIsInvalid = Error.Problem(
            "Students.EmailIsInvalid",
            "E-mail is in invalid format");

        public static readonly Error EmailMustBeNotEmpty = Error.Problem(
            "Students.EmailMustBeNotEmpty",
            "E-mail must be not empty");

        public static readonly Error BirthDateMustBeNotEmpty = Error.Problem(
            "Students.BirthDateMustBeNotEmpty",
            "Birth Data must be not empty");
    }
}