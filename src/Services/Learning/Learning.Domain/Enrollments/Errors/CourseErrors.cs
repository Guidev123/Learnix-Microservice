using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class CourseErrors
    {
        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
             "Courses.CourseIdMustBeNotEmpty",
             "Course ID must be not empty");

        public static readonly Error ProgressDateRangeMustBeNotNull = Error.Problem(
            "Courses.ProgressDateRangeMustBeNotNull",
            "Progress date range must be not null");

        public static readonly Error EnrollmentIdMustBeNotEmpty = Error.Problem(
            "Courses.EnrollmentIdMustBeNotEmpty",
            "Enrollment ID must be not empty");

        public static readonly Error ProgresStartedDateCanNotBeInFuture = Error.Problem(
            "Courses.LessonProgresStartedDateCannotBeInFuture",
            "Progress started date cannot be in the future");

        public static readonly Error ProgressCompletedDateMustBeAfterStartedDate = Error.Problem(
            "Courses.ProgressCompletedDateMustBeAfterStartedDate",
            "Progress completed date must be after the started date");

        public static readonly Error ModulesMustNotBeEmpty = Error.Problem(
            "Courses.ModulesMustNotBeEmpty",
            "Modules must not be empty");
    }
}