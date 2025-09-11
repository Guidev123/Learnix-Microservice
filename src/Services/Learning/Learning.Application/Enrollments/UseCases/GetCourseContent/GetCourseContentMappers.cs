using Learning.Domain.Enrollments.Entities;

namespace Learning.Application.Enrollments.UseCases.GetCourseContent
{
    internal static class GetCourseContentMappers
    {
        public static CourseContentResponse MapFromEntity(this Course course)
        {
            return new CourseContentResponse(
                course.Id,
                course.Title,
                course.Description,
                course.DificultLevel,
                course.Status,
                course.Modules.Select(m => m.MapFromEntity()).ToList());
        }

        public static ModuleResponse MapFromEntity(this Module module)
        {
            return new ModuleResponse(
                module.Id,
                module.Title,
                module.OrderIndex,
                module.CourseId,
                module.Lessons.Select(l => l.MapFromEntity()).ToList(),
                module.NextModuleId,
                module.PreviousModuleId);
        }

        public static LessonResponse MapFromEntity(this Lesson lesson)
        {
            return new LessonResponse(
                lesson.Id,
                lesson.Title,
                lesson.VideoUrl,
                lesson.OrderIndex,
                lesson.ModuleId,
                lesson.NextLessonId,
                lesson.PreviousLessonId);
        }
    }
}