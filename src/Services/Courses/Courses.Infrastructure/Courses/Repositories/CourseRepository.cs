using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Interfaces;
using Courses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Courses.Repositories
{
    internal sealed class CourseRepository(CourseDbContext context) : ICourseRepository
    {
        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Module?> GetModuleByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Modules.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        public async Task<Course?> GetWithModulesByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Courses.AsNoTrackingWithIdentityResolution().Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Course?> GetWithModulesAndLessonsAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Courses.AsNoTrackingWithIdentityResolution().Include(c => c.Modules).ThenInclude(c => c.Lessons).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public void Insert(Course course) => context.Courses.Add(course);

        public void InsertLessonsToModuleRange(IEnumerable<Lesson> lessons) => context.Lessons.AddRange(lessons);

        public void InsertModulesRange(IEnumerable<Module> modules) => context.Modules.AddRange(modules);

        public void Update(Course course) => context.Courses.Add(course);

        public void Dispose() => context.Dispose();
    }
}