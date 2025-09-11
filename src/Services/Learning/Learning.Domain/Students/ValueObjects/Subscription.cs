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
            Validate();
        }

        private Subscription()
        { }

        public SubscriptionTypeEnum Type { get; }
        public DateTime ExpiresAt { get; }

        public bool IsActive(DateTime currentDate) => ExpiresAt > currentDate;

        public static implicit operator Subscription((SubscriptionTypeEnum type, DateTime currentDate) subscriptionData)
        {
            var subscriptionFactoryDelegates = new Dictionary<SubscriptionTypeEnum, Func<DateTime, Subscription>>()
            {
                { SubscriptionTypeEnum.Premium, CreatePremium }
            };

            if (!subscriptionFactoryDelegates.TryGetValue(subscriptionData.type, out var func))
            {
                throw new DomainException(StudentErrors.SubscriptionTypeNotFound(subscriptionData.type).Description);
            }

            return func(subscriptionData.currentDate);
        }

        private static Subscription CreatePremium(DateTime currentDate)
            => new(SubscriptionTypeEnum.Premium, currentDate.AddDays(PremiumExpirationInDays));

        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(ExpiresAt > DateTime.UtcNow, StudentErrors.SubscriptionExpirationMustBeInFuture.Description);
        }
    }
}