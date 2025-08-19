using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users.IntegrationEvents;
using MidR.Interfaces;
using Users.Application.Users.UseCases.GetById;
using Users.Domain.Users.DomainEvents;

namespace Users.Application.Users.DomainEvents
{
    internal sealed class UserCreatedDomainEventHandler(IMediator mediator, IMessageBus messageBus) : DomainEventHandler<UserCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var userResult = await mediator.SendAsync(new GetUserByIdQuery(domainEvent.UserId), cancellationToken);
            if (userResult.IsFailure)
            {
                throw new LearnixException(nameof(GetUserByIdQuery), userResult.Error);
            }

            var user = userResult.Value;

            await messageBus.ProduceAsync("users.user-created", new UserCreatedIntegrationEvent(
                domainEvent.CorrelationId,
                domainEvent.OccurredOn,
                domainEvent.UserId,
                user.FirstName,
                user.LastName,
                user.Email), cancellationToken);
        }
    }
}