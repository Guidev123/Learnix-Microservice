namespace Learnix.Commons.Infrastructure.MessageBus
{
    public sealed class MessageBusOptions
    {
        public string BootstrapServer { get; set; } = null!;
        public string GroupId { get; set; } = null!;
    }
}