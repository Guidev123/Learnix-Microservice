using Learnix.Commons.Domain.Abstractions;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);

        void Insert(User user);

        void Update(User user);

        void Delete(User user);
    }
}