using Microsoft.EntityFrameworkCore;
using Users.Domain.Users.Entities;
using Users.Domain.Users.Interfaces;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.Users.Repositories
{
    internal sealed class UserRepository(UsersDbContext context) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Users.AsNoTracking().AnyAsync(u => u.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
            => await context.Users.AsNoTracking().AnyAsync(u => u.Email.Address == email, cancellationToken);

        public void Insert(User user)
        {
            foreach (var role in user.Roles)
            {
                context.Attach(role);
            }

            context.Users.Add(user);
        }

        public void Update(User user)
        {
            context.Users.Update(user);
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
        }
    }
}