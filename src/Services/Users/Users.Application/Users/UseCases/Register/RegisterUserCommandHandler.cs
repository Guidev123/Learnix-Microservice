﻿using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;
using Users.Application.Abstractions.Identity;
using Users.Domain.Entities;
using Users.Domain.Errors;
using Users.Domain.Interfaces;

namespace Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserCommandHandler(
        IIdentityProviderService identityProviderService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        public async Task<Result<RegisterUserResponse>> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
        {
            var userExists = await userRepository.ExistsAsync(request.Email, cancellationToken);
            if (userExists)
            {
                return Result.Failure<RegisterUserResponse>(UserErrors.AlreadyExists(request.Email));
            }

            var userResult = User.Create(request.FullName, request.Email, request.BirthDate);

            var user = userResult.Value;

            var identityResult = await identityProviderService.RegisterAsync(new(
                user.Email.Address,
                user.Name.FirstName,
                user.Name.LastName,
                request.Password), cancellationToken);

            if (identityResult.IsFailure)
            {
                return Result.Failure<RegisterUserResponse>(identityResult.Error!);
            }

            if (!Guid.TryParse(identityResult.Value, out Guid identityId))
            {
                return Result.Failure<RegisterUserResponse>(IdentityProviderErrors.FailToGetIdentityId);
            }

            user.SetIdentityId(identityId);

            userRepository.Insert(user);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success(new RegisterUserResponse(user.Id))
                : Result.Failure<RegisterUserResponse>(UserErrors.SomethingHasFailedDuringSavingChanges);
        }
    }
}