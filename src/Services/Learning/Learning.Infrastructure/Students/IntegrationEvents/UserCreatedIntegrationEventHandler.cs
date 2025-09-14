using Learning.Application.Features.CreateStudent;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users.IntegrationEvents;
using MidR.Interfaces;

namespace Learning.Infrastructure.Students.IntegrationEvents
{
    internal sealed class UserCreatedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var command = new CreateStudentCommand(notification.UserId, notification.FirstName, notification.LastName, notification.Email);

            var result = await sender.SendAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                throw new LearnixException(nameof(CreateStudentCommand), result.Error);
            }
        }
    }
}