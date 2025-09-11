using Learnix.Commons.Domain.Results;

namespace Learning.Domain.Enrollments.Errors
{
    public static class LessonErrors
    {
        public static readonly Error LessonIdMustBeNotEmpty = Error.Problem(
            "Lessons.LessonIdMustBeNotEmpty",
            "Lesson ID must be not empty");
    }
}