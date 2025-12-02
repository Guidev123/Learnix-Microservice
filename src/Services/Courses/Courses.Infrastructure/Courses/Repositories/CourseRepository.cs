using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Interfaces;
using Courses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Courses.Repositories
{
    internal sealed class CourseRepository(CoursesDbContext context) : ICourseRepository
    {
        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Module?> GetModuleByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Modules.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        public async Task<Course?> GetWithModulesByIdAsync(Guid id, bool asNoTrackingEnabled = true, CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery(asNoTrackingEnabled);

            return await query
                .Include(c => c.Modules.OrderBy(m => m.OrderIndex))
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Course?> GetWithModulesAndLessonsAsync(Guid id, bool asNoTrackingEnabled = true, CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery(asNoTrackingEnabled);

            return await query
                .AsSplitQuery()
                .Include(c => c.Modules.OrderBy(m => m.OrderIndex))
                .ThenInclude(m => m.Lessons.OrderBy(l => l.OrderIndex))
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public void Insert(Course course) => context.Courses.Add(course);

        public void InsertLessonsToModuleRange(IEnumerable<Lesson> lessons) => context.Lessons.AddRange(lessons);

        public void InsertModulesRange(IEnumerable<Module> modules) => context.Modules.AddRange(modules);

        public void Update(Course course) => context.Courses.Update(course);

        private IQueryable<Course> BuildBaseQuery(bool asNoTrackingEnabled)
        {
            var query = context.Courses.AsQueryable();

            return asNoTrackingEnabled
                ? query.AsNoTrackingWithIdentityResolution()
                : query;
        }
    }
}