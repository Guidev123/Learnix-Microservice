using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.AttachCourse
{
    public sealed record AttachCourseCommand(
        Guid Id,
        string Title,
        string Description,
        string DificultLevel,
        string Status,
        List<ModuleRequest> Modules) : ICommand;
}