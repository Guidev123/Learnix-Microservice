namespace Learning.Application.Enrollments.UseCases.GetCourseContent
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