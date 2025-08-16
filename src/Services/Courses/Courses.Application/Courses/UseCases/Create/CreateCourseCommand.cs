using Courses.Domain.Courses.Enumerators;
using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.Create
{
    public sealed record CreateCourseCommand(
        string Title,
        string Description,
        DificultLevelEnum DificultLevel,
        Guid CategoryId
        ) : ICommand<CreateCourseResponse>;
}