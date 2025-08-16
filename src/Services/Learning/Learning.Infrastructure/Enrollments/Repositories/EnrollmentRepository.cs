using Dapper;
using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Enrollments.Interfaces;
using Learning.Infrastructure.Persistence;
using Learnix.Commons.Application.Factories;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Enrollments.Repositories
{
    internal sealed class EnrollmentRepository(
        LearningDbContext context,
        ISqlConnectionFactory sqlConnectionFactory
        ) : IEnrollmentRepository
    {
        public async Task<bool> AlreadyEnrolledAsync(Guid enrollmentId, Guid studentId, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $"""
                SELECT CASE
                    WHEN EXISTS(
                        SELECT 1
                        FROM {Schemas.Learning}.Enrollments
                        WHERE Id = @enrollmentId
                        AND StudentId = @studentId)
                   THEN 1
                   ELSE 0
                END
                """;

            return await connection.ExecuteScalarAsync<bool>(sql, new { enrollmentId, studentId });
        }

        public async Task<Enrollment?> GetByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
            => await context.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);

        public void Insert(Enrollment enrollment) => context.Enrollments.Add(enrollment);

        public void Update(Enrollment enrollment) => context.Update(enrollment);

        public void Dispose() => context.Dispose();

        public Task<bool> CourseAlreadyExistsAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}