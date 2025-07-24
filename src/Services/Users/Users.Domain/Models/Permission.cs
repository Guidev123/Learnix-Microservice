namespace Users.Domain.Models
{
    public sealed class Permission
    {
        public static readonly Permission GetUser = new("users:read");
        public static readonly Permission UpdateUser = new("users:update");
        public static readonly Permission DeleteUser = new("users:delete");
        public static readonly Permission BuySubscription = new("subscriptions:buy-subscription");

        public Permission(string code) => Code = code;

        public string Code { get; }
    }
}