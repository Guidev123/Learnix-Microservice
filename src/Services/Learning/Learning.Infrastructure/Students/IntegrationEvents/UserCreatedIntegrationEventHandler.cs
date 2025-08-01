using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users;

namespace Learning.Infrastructure.Students.IntegrationEvents
{
    internal sealed class UserCreatedIntegrationEventHandler : IntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
        }
    }
}