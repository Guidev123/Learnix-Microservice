using Learning.Domain.Enrollments.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Learning.Domain.Enrollments.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment?> GetByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default);

        Task<bool> AlreadyEnrolledAsync(Guid enrollmentId, Guid studentId, CancellationToken cancellationToken = default);

        void Insert(Enrollment enrollment);

        void Update(Enrollment enrollment);
    }
}