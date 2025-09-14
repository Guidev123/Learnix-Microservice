namespace Learning.Application.Features.GetCourseContent
{
    public sealed record CourseContentResponse(
        Guid Id,
        string Title,
        string Description,
        string DificultLevel,
        string Status,
        List<ModuleResponse> Modules);
}