namespace Learnix.Commons.WebApi.Extensions
{
    public static class PolicyExtensions
    {
        public static readonly string GetUser = "users:read";
        public static readonly string UpdateUser = "users:update";
        public static readonly string DeleteUser = "users:delete";
        public static readonly string BuySubscription = "subscriptions:buy-subscription";
        public static readonly string EnrollStudent = "learning:enroll-student";
        public static readonly string CreateCourse = "courses:create";
        public static readonly string AttachModules = "courses:attach-modules";
        public static readonly string AttachLessonsToModule = "courses:attach-lessons-to-module";
        public static readonly string GetCourseContent = "courses:read-content";
    }
}