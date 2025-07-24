using Learnix.Commons.Domain.Results;
using MidR.MemoryQueue.Interfaces;

namespace Learnix.Commons.Application.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}