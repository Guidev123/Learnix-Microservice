using Learning.Domain.Enrollments.Entities;

namespace Learning.Application.Features.AttachCourse
{
    internal static class AttachCourseMappers
    {
        public static Course MapToEntity(this AttachCourseCommand command)
        {
            var modules = command.Modules
                .Select(m => Module.Create(
                    m.Id,
                    m.Title,
                    m.OrderIndex,
                    m.CourseId,
                    m.NextModuleId,
                    m.PreviousModuleId,
                    m.Lessons
                        .Select(l => Lesson.Create(
                            l.Id,
                            l.Title,
                            l.VideoUrl,
                            l.OrderIndex,
                            l.ModuleId,
                            l.NextLessonId,
                            l.PreviousLessonId
                            )).ToList()
                    ))
                .ToList();

            return Course.Create(
                command.Id,
                command.Title,
                command.Description,
                command.DificultLevel,
                command.Status,
                modules
                );
        }
    }
}