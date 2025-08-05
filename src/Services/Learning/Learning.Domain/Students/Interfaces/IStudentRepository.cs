using Learning.Domain.Students.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Learning.Domain.Students.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);

        Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Student student);

        void Update(Student student);

        void Delete(Student student);
    }
}