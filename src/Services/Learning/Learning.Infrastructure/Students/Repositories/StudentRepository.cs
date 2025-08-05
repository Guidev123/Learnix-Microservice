using Learning.Domain.Students.Entities;
using Learning.Domain.Students.Interfaces;
using Learning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Students.Repositories
{
    internal sealed class StudentRepository(LearningDbContext context) : IStudentRepository
    {
        public Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
            => context.Students.AsNoTracking().AnyAsync(s => s.Email.Address == email, cancellationToken);

        public async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public void Insert(Student student) => context.Students.Add(student);

        public void Update(Student student) => context.Students.Update(student);

        public void Delete(Student student) => context.Students.Remove(student);

        public void Dispose() => context.Dispose();
    }
}