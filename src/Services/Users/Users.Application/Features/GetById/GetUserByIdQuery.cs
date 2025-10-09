using Learnix.Commons.Application.Messaging;

namespace Users.Application.Features.GetById
{
    public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;
}