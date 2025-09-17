using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Enumerators;
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
            var course = await courseRepository.GetWithModulesByIdAsync(request.CourseId, false, cancellationToken);
            if (course is null)
            {
                return Result.Failure(CourseErrors.NotFound(request.CourseId));
            }

            if (course.Status == CourseStatusEnum.Published)
            {
                return Result.Failure(CourseErrors.StatusMustBeNotPublished(request.CourseId));
            }

            var modules = request.Modules.Select(m => Module.Create(m.Title, course.Id));
            course.AddModules(modules);

            courseRepository.InsertModulesRange(course.Modules);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);

            return wasSaved
                ? Result.Success()
                : Result.Failure(ModuleErrors.FailToPersistModules);
        }
    }
}