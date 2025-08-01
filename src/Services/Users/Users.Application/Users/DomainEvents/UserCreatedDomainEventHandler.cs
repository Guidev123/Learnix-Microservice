using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users;
using MidR.MemoryQueue.Interfaces;
using Users.Application.Users.UseCases.GetById;
using Users.Domain.DomainEvents;

namespace Users.Application.Users.DomainEvents
{
    internal sealed class UserCreatedDomainEventHandler(IMediator mediator, IMessageBus messageBus) : DomainEventHandler<UserCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var userResult = await mediator.DispatchAsync(new GetUserByIdQuery(notification.UserId), cancellationToken);
            if (userResult.IsFailure)
            {
                throw new LearnixException(nameof(GetUserByIdQuery), userResult.Error);
            }

            var user = userResult.Value;

            await messageBus.ProduceAsync("users.user-created", new UserCreatedIntegrationEvent(
                notification.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.BirthDate
                ), cancellationToken);
        }
    }
}