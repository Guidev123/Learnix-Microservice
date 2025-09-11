using Learning.Domain.Students.Enumerators;
using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Students.Errors
{
    public static class StudentErrors
    {
        public static Error NotFound(Guid studentId) => Error.NotFound(
            "Students.NotFound",
            $"Student with ID: {studentId} not found");

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

        public static readonly Error EmailMustBeNotNull = Error.Problem(
            "Students.EmailMustBeNotNull",
            "E-mail must be not null");

        public static readonly Error NameMustBeNotNull = Error.Problem(
             "Students.NameMustBeNotNull",
                  "Name must be not null");

        public static readonly Error IdMustBeNotEmpty = Error.Problem(
            "Students.IdMustBeNotEmpty",
            "Id must be not empty");

        public static Error StudentAlreadyExists(string email) => Error.Conflict(
            "Students.StudentAlreadyExists",
            $"A student with the email '{email}' already exists.");

        public static Error PersistenceError(Guid id) => Error.Failure(
            "Students.PersistenceError",
            $"An error occurred while saving the student with ID '{id}' to the database.");

        public static Error SubscriptionTypeNotFound(SubscriptionTypeEnum subscriptionTypeEnum) => Error.NotFound(
            "Subscriptions.SubscriptionTypeNotFound",
            $"The subscription type {subscriptionTypeEnum} not found");

        public static readonly Error ToEnrollYouNeedSubscription = Error.Problem(
            "Students.ToEnrollYouNeedSubscription",
            "To enroll in a course you need a subscription");

        public static readonly Error SubscriptionExpirationMustBeInFuture = Error.Problem(
            "Students.SubscriptionExpirationMustBeInFuture",
            "Subscription expiration date must be in the future");
    }
}