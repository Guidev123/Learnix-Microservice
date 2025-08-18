using Courses.Domain.Courses.Entities;
using Learnix.Commons.Domain.Abstractions;

namespace Courses.Domain.Courses.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Module?> GetModuleByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Course?> GetWithModulesByIdAsync(Guid id, bool asNoTrackingEnabled = true, CancellationToken cancellationToken = default);

        Task<Course?> GetWithModulesAndLessonsAsync(Guid id, bool asNoTrackingEnabled = true, CancellationToken cancellationToken = default);

        void Insert(Course course);

        void Update(Course course);

        void InsertModulesRange(IEnumerable<Module> modules);

        void InsertLessonsToModuleRange(IEnumerable<Lesson> lessons);
    }
}