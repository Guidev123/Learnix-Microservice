using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class CourseProgressErrors
    {
        public static Error NotFoundForCustomerId(Guid courseProgressId) => Error.NotFound
            ("CoursesProgress.NotFoundForCustomerId",
            $"Course Progress with ID '{courseProgressId}' was not found for student with ID '{courseProgressId}'");

        public static Error NotFoundForEnrollmentId(Guid enrollmentId) => Error.NotFound
            ("CoursesProgress.NotFoundForEnrollmentId",
            $"Course Progress with Enrollment ID '{enrollmentId}' was not found");

        public static Error NotFoundModuleProgress(Guid moduleProgressId) => Error.NotFound
            ("CoursesProgress.NotFoundModuleProgress",
            $"Module Progress with ID '{moduleProgressId}' was not found");

        public static Error NotFoundLessonProgress(Guid lessonProgressId) => Error.NotFound
            ("CoursesProgress.NotFoundLessonProgress",
            $"Lesson Progress with ID '{lessonProgressId}' was not found");

        public static readonly Error LessonMustBeStartedBeforeCompletion = Error.Problem(
            "CoursesProgress.LessonMustBeStartedBeforeCompletion",
            "Lesson must be started before it can be completed");

        public static readonly Error StudentIdMustBeNotEmpty = Error.Problem(
            "CoursesProgress.StudentIdMustBeNotEmpty",
            "Student ID must be not empty");

        public static readonly Error EnrollmentIdMustBeNotEmpty = Error.Problem(
            "CoursesProgress.EnrollmentIdMustBeNotEmpty",
            "Enrollment ID must be not empty");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Problem(
            "CoursesProgress.CourseIdMustBeNotEmpty",
            "Course ID must be not empty");

        public static readonly Error ProgresStartedDateCanNotBeInFuture = Error.Problem(
             "CoursesProgress.ProgresStartedDateCanNotBeInFuture",
            "Progress started date cannot be in the future");

        public static readonly Error ProgressCompletedDateMustBeAfterStartedDate = Error.Problem(
            "CoursesProgress.ProgressCompletedDateMustBeAfterStartedDate",
            "Progress completed date must be after started date");

        public static readonly Error FailToPersistChanges = Error.Problem(
            "CourseProgress.FailToPersistChanges",
            "Fail to persist course progress changes");
    }
}