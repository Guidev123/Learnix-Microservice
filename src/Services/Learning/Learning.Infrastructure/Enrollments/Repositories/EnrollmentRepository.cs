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
        private readonly IMongoCollection<Course> _collection;

        public EnrollmentRepository(
            IMongoClient mongoClient,
            LearningDbContext context
        )
        {
            var mongoDatabase = mongoClient.GetDatabase(DocumentDbSettings.Database);

            _collection = mongoDatabase.GetCollection<Course>(DocumentDbSettings.CoursesContent);
            _context = context;
        }

        public Task<bool> AlreadyEnrolledAsync(Guid courseId, Guid studentId, CancellationToken cancellationToken = default)
        {
            return _context.Enrollments
                .AsNoTracking()
                .AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId, cancellationToken);
        }

        public Task<Enrollment?> GetByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
            => _context.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);

        public Task<Enrollment?> GetByStudentAndCourseIdAsync(Guid studentId, Guid courseId, CancellationToken cancellationToken = default)
            => _context.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);

        public void Insert(Enrollment enrollment) => _context.Enrollments.Add(enrollment);

        public void Update(Enrollment enrollment) => _context.Update(enrollment);

        public Task<Course> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
            => _collection.Find(Builders<Course>.Filter.Eq(c => c.Id, courseId)).FirstOrDefaultAsync(cancellationToken);

        public Task InsertCourseAsync(Course course, CancellationToken cancellationToken = default)
            => _collection.InsertOneAsync(course, cancellationToken: cancellationToken);

        public Task ReplaceCourseAsync(Course course, CancellationToken cancellationToken = default)
            => _collection.ReplaceOneAsync(Builders<Course>.Filter.Eq(c => c.Id, course.Id), course, cancellationToken: cancellationToken);

        public async Task<bool> CourseExistsAsync(Guid courseId, CancellationToken cancellationToken = default)
            => await _collection.CountDocumentsAsync(Builders<Course>.Filter.Eq(c => c.Id, courseId), cancellationToken: cancellationToken) > 0;
    }
}