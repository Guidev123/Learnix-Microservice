namespace Learning.Application.Enrollments.UseCases.AttachCourse
{
    public sealed record ModuleRequest(
        Guid Id,
        string Title,
        uint OrderIndex,
        List<LessonRequest> Lessons,
        Guid CourseId,
        Guid? NextModuleId,
        Guid? PreviousModuleId);
}