using Learnix.Commons.Domain.Results;

namespace Users.Domain.Errors
{
    public static class UserErrors
    {
        public static Error NotFound(Guid id)
            => Error.NotFound(
                "Users.NotFound",
                $"User with ID: {id} not found");

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

        public static readonly Error PasswordIsRequired = Error.Problem(
            "Users.PasswordIsRequired",
            "The password field cannot be empty");

        public static readonly Error PasswordTooShort = Error.Problem(
            "Users.PasswordTooShort",
            "The password must be at least 8 characters long");

        public static readonly Error PasswordMissingUpperCase = Error.Problem(
            "Users.PasswordMissingUpperCase",
            "The password must contain at least one uppercase letter");

        public static readonly Error PasswordMissingLowerCase = Error.Problem(
            "Users.PasswordMissingLowerCase",
            "The password must contain at least one lowercase letter");

        public static readonly Error PasswordMissingDigit = Error.Problem(
            "Users.PasswordMissingDigit",
            "The password must contain at least one digit");

        public static readonly Error PasswordMissingSpecialChar = Error.Problem(
            "Users.PasswordMissingSpecialChar",
            "The password must contain at least one special character (!@#$%^&* etc.)");

        public static readonly Error PasswordsDoNotMatch = Error.Problem(
            "Users.PasswordsDoNotMatch",
            "The passwords do not match");

        public static Error PermissionNotFoundForIdenityId(string idenityId)
            => Error.NotFound(
                "Users.PermissionNotFoundForIdenityId",
                $"No role found for user with identity identifier: {idenityId}");

        public static Error AlreadyExists(string email) => Error.Conflict(
            "Users.AlreadyExists",
            $"User with E-mail: {email} already exists");
    }
}