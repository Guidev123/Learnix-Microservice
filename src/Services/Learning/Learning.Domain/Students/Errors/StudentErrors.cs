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

        public static readonly Error EmailMustBeNotNull = Error.Problem(
            "Students.EmailMustBeNotNull",
            "E-mail must be not null");

        public static readonly Error NameMustBeNotNull = Error.Problem(
             "Students.NameMustBeNotNull",
                  "Name must be not null");

        public static readonly Error IdMustBeNotEmpty = Error.Problem(
            "Students.IdMustBeNotEmpty",
            "Id must be not empty");
    }
}