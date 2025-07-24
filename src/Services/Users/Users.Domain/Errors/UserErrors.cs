using Learnix.Commons.Domain.Results;

namespace Users.Domain.Errors
{
    public static class UserErrors
    {
        public static readonly Error NameMustBeNotNull = Error.Problem(
            "Users.NameMustBeNotNull",
            "User name must be not null");

        public static readonly Error EmailMustBeNotNull = Error.Problem(
            "Users.EmailMustBeNotNull",
            "User E-mail must be not null");

        public static readonly Error AgeMustBeNotNull = Error.Problem(
            "Users.AgeMustBeNotNull",
            "User Age must be not null");

        public static readonly Error IdentityIdMustBeNotNull = Error.Problem(
            "Users.IdentityIdMustBeNotNull",
            "Identity Id must be not null");

        public static readonly Error IdentityIdMustBeNotEmpty = Error.Problem(
            "Users.IdentityIdMustBeNotEmpty",
            "Identity Id must be not empty");

        public static readonly Error SomethingHasFailedDuringSavingChanges = Error.Problem(
            "Users.SomethingHasFailedDuringSavingChanges",
            "Something has failed during saving changes of user");
    }
}