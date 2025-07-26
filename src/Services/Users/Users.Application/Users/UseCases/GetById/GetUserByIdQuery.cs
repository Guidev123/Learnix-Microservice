using Learnix.Commons.Application.Messaging;

namespace Users.Application.Users.UseCases.GetById
{
    public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;
}