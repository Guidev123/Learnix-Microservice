namespace Learning.Application.Enrollments.UseCases.AttachCourse
{
    public sealed record LessonRequest(
        Guid Id,
        string Title,
        string VideoUrl,
        uint OrderIndex,
        Guid ModuleId,
        Guid? NextLessonId,
        Guid? PreviousLessonId);
}