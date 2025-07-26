using MidR.MemoryQueue.Interfaces;

namespace Learnix.Commons.Domain.DomainObjects
{
    public interface IEvent : INotification
    {
        Guid CorrelationId { get; }
        DateTime OccurredOn { get; }
        string Messagetype { get; }
    }
}