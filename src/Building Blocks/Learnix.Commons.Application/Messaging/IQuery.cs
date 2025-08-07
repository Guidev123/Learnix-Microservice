using Learnix.Commons.Domain.Results;
using MidR.Interfaces;

namespace Learnix.Commons.Application.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}