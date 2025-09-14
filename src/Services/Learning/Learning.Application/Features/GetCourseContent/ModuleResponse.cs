namespace Learning.Application.Features.GetCourseContent
{
    public sealed record ModuleResponse(
        Guid Id,
        string Title,
        uint OrderIndex,
        Guid CourseId,
        List<LessonResponse> Lessons,
        Guid? NextModuleId,
        Guid? PreviousModuleId);
}