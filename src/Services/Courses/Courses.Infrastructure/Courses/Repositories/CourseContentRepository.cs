using Courses.Application.Courses.Abstractions;
using Courses.Application.Courses.UseCases.GetContent;
using Courses.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Courses.Infrastructure.Courses.Repositories
{
    internal sealed class CourseContentRepository : ICourseContentRepository
    {
        private readonly IMongoCollection<CourseContentResponse> _collection;

        public CourseContentRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(DocumentDbSettings.Database);

            _collection = mongoDatabase.GetCollection<CourseContentResponse>(DocumentDbSettings.CoursesContent);
        }

        public async Task<CourseContentResponse> GetByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
            => await _collection.Find(Builders<CourseContentResponse>.Filter.Eq(c => c.Id, courseId)).FirstOrDefaultAsync(cancellationToken);

        public async Task InsertAsync(CourseContentResponse response, CancellationToken cancellationToken = default)
            => await _collection.InsertOneAsync(response, cancellationToken: cancellationToken);

        public async Task ReplaceAsync(CourseContentResponse response, CancellationToken cancellationToken = default)
            => await _collection.ReplaceOneAsync(Builders<CourseContentResponse>.Filter.Eq(c => c.Id, response.Id), response, cancellationToken: cancellationToken);
    }
}