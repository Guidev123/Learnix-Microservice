using Learnix.Commons.Domain.Results;
using MidR.MemoryQueue.Interfaces;

namespace Learnix.Commons.Application.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}