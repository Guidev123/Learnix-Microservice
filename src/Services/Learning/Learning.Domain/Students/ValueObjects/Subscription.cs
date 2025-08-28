using Learning.Domain.Students.Enumerators;
using Learning.Domain.Students.Errors;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Learning.Domain.Students.ValueObjects
{
    public sealed record Subscription : ValueObject
    {
        public const int PremiumExpirationInDays = 365;

        private Subscription(SubscriptionTypeEnum type, DateTime expiresAt)
        {
            Type = type;
            ExpiresAt = expiresAt;
        }

        private Subscription()
        { }

        public SubscriptionTypeEnum Type { get; }
        public DateTime ExpiresAt { get; }

        public bool IsActive(DateTime currentDate) => ExpiresAt > currentDate;

        public static Subscription Create(SubscriptionTypeEnum type, DateTime currentDate)
        {
            var subscriptionFactoryDelegates = new Dictionary<SubscriptionTypeEnum, Func<DateTime, Subscription>>()
            {
                { SubscriptionTypeEnum.Premium, CreatePremium }
            };

            if (!subscriptionFactoryDelegates.TryGetValue(type, out var func))
            {
                throw new DomainException(StudentErrors.SubscriptionTypeNotFound(type).Description);
            }

            return func(currentDate);
        }

        private static Subscription CreatePremium(DateTime currentDate)
        {
            return new Subscription(SubscriptionTypeEnum.Premium, currentDate.AddDays(PremiumExpirationInDays));
        }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}