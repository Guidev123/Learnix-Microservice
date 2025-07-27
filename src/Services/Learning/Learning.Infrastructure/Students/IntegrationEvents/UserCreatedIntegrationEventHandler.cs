using Learnix.Commons.Contracts.Users;
using MidR.MemoryQueue.Interfaces;

namespace Learning.Infrastructure.Students.IntegrationEvents
{
    internal sealed class UserCreatedIntegrationEventHandler : INotificationHandler<UserCreatedIntegrationEvent>
    {
        public async Task ExecuteAsync(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
        }
    }
}