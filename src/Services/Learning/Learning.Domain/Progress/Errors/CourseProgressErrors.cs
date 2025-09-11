using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Progress.Errors
{
    public static class CourseProgressErrors
    {
        public static Error NotFound(Guid courseId) => Error.NotFound
            ("CoursesProgress.NotFound",
            $"Course with ID '{courseId}' was not found");

        public static readonly Error LessonMustBeStartedBeforeCompletion = Error.Failure(
            "CoursesProgress.LessonMustBeStartedBeforeCompletion",
            "Lesson must be started before it can be completed");

        public static readonly Error StudentIdMustBeNotEmpty = Error.Failure(
            "CoursesProgress.StudentIdMustBeNotEmpty",
            "Student ID must be not empty");

        public static readonly Error EnrollmentIdMustBeNotEmpty = Error.Failure(
            "CoursesProgress.EnrollmentIdMustBeNotEmpty",
            "Enrollment ID must be not empty");

        public static readonly Error CourseIdMustBeNotEmpty = Error.Failure(
            "CoursesProgress.CourseIdMustBeNotEmpty",
            "Course ID must be not empty");

        public static readonly Error ProgresStartedDateCanNotBeInFuture = Error.Failure(
             "CoursesProgress.ProgresStartedDateCanNotBeInFuture",
            "Progress started date cannot be in the future");

        public static readonly Error ProgressCompletedDateMustBeAfterStartedDate = Error.Failure(
            "CoursesProgress.ProgressCompletedDateMustBeAfterStartedDate",
            "Progress completed date must be after started date");
    }
}