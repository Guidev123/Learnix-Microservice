using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.Abstractions
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
    }
}