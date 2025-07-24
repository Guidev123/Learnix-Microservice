﻿namespace Learnix.Commons.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    }
}