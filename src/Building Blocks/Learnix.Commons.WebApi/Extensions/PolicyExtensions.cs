namespace Learnix.Commons.WebApi.Extensions
{
    public static class PolicyExtensions
    {
        public static readonly string GetUser = "users:read";
        public static readonly string UpdateUser = "users:update";
        public static readonly string DeleteUser = "users:delete";
        public static readonly string BuySubscription = "subscriptions:buy-subscription";
        public static readonly string EnrollStudent = "learning:enroll-student";
    }
}