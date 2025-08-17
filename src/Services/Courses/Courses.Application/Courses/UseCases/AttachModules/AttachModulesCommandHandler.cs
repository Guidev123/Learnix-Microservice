using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Courses.UseCases.AttachModules
{
    internal sealed class AttachModulesCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AttachModulesCommand>
    {
        public async Task<Result> ExecuteAsync(AttachModulesCommand request, CancellationToken cancellationToken = default)
        {
            var course = await courseRepository.GetWithModulesByIdAsync(request.CourseId, cancellationToken);
            if (course is null)
            {
                return Result.Failure(CourseErrors.NotFound(request.CourseId));
            }

            var index = GetLastOrderIndex(course);

            var modules = request.Modules.Select(m => Module.Create(m.Title, course.Id));
            AddModulesToCourse(modules, course);

            var orderedModules = course.Modules.Where(x => x.OrderIndex > index);
            courseRepository.InsertModulesRange(orderedModules);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success()
                : Result.Failure(ModuleErrors.FailToPersistModules);
        }

        private static void AddModulesToCourse(IEnumerable<Module> modules, Course course)
        {
            foreach (var module in modules)
            {
                course.AddModule(module);
            }
        }

        private static uint GetLastOrderIndex(Course course)
            => course.GetModulesInOrder().Select(x => x.OrderIndex).LastOrDefault();
    }
}