using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Subscriptions.Domain.Subscriptions.Entities
{
    public sealed class Subscription : Entity, IAggregateRoot
    {
        private Subscription()
        { }

        public Money Price { get; private set; } = null!;


        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}