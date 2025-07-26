using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Entities
{
    public sealed class Student : Entity, IAggregateRoot
    {
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}