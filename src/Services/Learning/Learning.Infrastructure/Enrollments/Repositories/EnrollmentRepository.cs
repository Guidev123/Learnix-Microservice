using Dapper;
using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Enrollments.Interfaces;
using Learning.Infrastructure.Persistence;
using Learnix.Commons.Application.Factories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Learning.Infrastructure.Enrollments.Repositories
{
    internal sealed class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly LearningDbContext _context;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IMongoCollection<Course> _collection;

        public EnrollmentRepository(
            IMongoClient mongoClient,
            LearningDbContext context,
            ISqlConnectionFactory sqlConnectionFactory
        )
        {
            var mongoDatabase = mongoClient.GetDatabase(DocumentDbSettings.Database);

            _collection = mongoDatabase.GetCollection<Course>(DocumentDbSettings.CoursesContent);
            _context = context;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<bool> AlreadyEnrolledAsync(Guid enrollmentId, Guid studentId, CancellationToken cancellationToken = default)
        {
            using var connection = _sqlConnectionFactory.Create();

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
            => await _context.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);

        public async Task<Enrollment?> GetByStudentAndCourseIdAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default)
            => await _context.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);

        public void Insert(Enrollment enrollment) => _context.Enrollments.Add(enrollment);

        public void Update(Enrollment enrollment) => _context.Update(enrollment);

        public async Task<Course> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
            => await _collection.Find(Builders<Course>.Filter.Eq(c => c.Id, courseId)).FirstOrDefaultAsync(cancellationToken);

        public async Task InsertCourseAsync(Course course, CancellationToken cancellationToken = default)
            => await _collection.InsertOneAsync(course, cancellationToken: cancellationToken);

        public async Task ReplaceCourseAsync(Course course, CancellationToken cancellationToken = default)
            => await _collection.ReplaceOneAsync(Builders<Course>.Filter.Eq(c => c.Id, course.Id), course, cancellationToken: cancellationToken);

        public async Task<bool> CourseExistsAsync(Guid courseId, CancellationToken cancellationToken = default)
            => await _collection.CountDocumentsAsync(Builders<Course>.Filter.Eq(c => c.Id, courseId), cancellationToken: cancellationToken) > 0;

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}