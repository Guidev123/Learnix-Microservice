using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Progress.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Learning.Domain.Enrollments.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment?> GetByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default);

        Task<Enrollment?> GetByStudentAndCourseIdAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default);

        Task<bool> AlreadyEnrolledAsync(Guid courseId, Guid studentId, CancellationToken cancellationToken = default);

        void Insert(Enrollment enrollment);

        void Update(Enrollment enrollment);

        Task<Course> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default);

        Task<bool> CourseExistsAsync(Guid courseId, CancellationToken cancellationToken = default);

        Task InsertCourseAsync(Course course, CancellationToken cancellationToken = default);

        Task ReplaceCourseAsync(Course course, CancellationToken cancellationToken = default);
    }
}