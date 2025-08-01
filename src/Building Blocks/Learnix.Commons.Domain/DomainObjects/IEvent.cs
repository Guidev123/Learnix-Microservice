namespace Learnix.Commons.Domain.DomainObjects
{
    public interface IEvent
    {
        Guid CorrelationId { get; }
        DateTime OccurredOn { get; }
        string Messagetype { get; }
    }
}