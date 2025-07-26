using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;
using Users.Domain.Errors;
using Users.Domain.Interfaces;

namespace Users.Application.Users.UseCases.GetById
{
    internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        public async Task<Result<GetUserByIdResponse>> ExecuteAsync(GetUserByIdQuery request, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
            {
                return Result.Failure<GetUserByIdResponse>(UserErrors.NotFound(request.Id));
            }

            var result = new GetUserByIdResponse(
                user.Id,
                user.Name.FirstName,
                user.Name.LastName,
                user.Email.Address,
                user.Age.BirthDate);

            return result;
        }
    }
}