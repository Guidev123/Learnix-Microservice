using Learnix.Commons.Domain.Results;
using MidR.MemoryQueue.Interfaces;

namespace Learnix.Commons.Application.Abstractions
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>;
}