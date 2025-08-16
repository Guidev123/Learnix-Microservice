using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class CourseProgressErrors
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            "CourseProgress.NotFound",
            $"CourseProgress with ID: {id} not found");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
             "CourseProgress.CourseIdMustBeNotEmpty",
             "CourseProgress ID must be not empty");

        public static readonly Error ProgressDateRangeMustBeNotNull = Error.Problem(
            "CourseProgress.ProgressDateRangeMustBeNotNull",
            "Progress date range must be not null");

        public static readonly Error ProgresStartedDateCanNotBeInFuture = Error.Problem(
            "CourseProgress.LessonProgresStartedDateCannotBeInFuture",
            "Progress started date cannot be in the future");

        public static readonly Error ProgressCompletedDateMustBeAfterStartedDate = Error.Problem(
            "CourseProgress.ProgressCompletedDateMustBeAfterStartedDate",
            "Progress completed date must be after the started date");

        public static readonly Error ModulesMustNotBeEmpty = Error.Problem(
            "CourseProgress.ModulesMustNotBeEmpty",
            "ModuleProgress must not be empty");
    }
}