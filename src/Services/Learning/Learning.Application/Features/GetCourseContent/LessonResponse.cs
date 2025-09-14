namespace Learning.Application.Features.GetCourseContent
{
    public sealed record LessonResponse(
        Guid Id,
        string Title,
        string VideoUrl,
        uint OrderIndex,
        Guid ModuleId,
        Guid? NextLessonId,
        Guid? PreviousLessonId);
}