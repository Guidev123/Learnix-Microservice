using Learning.Domain.Enrollments.Entities;

namespace Learning.Application.Enrollments.UseCases.AttachCourse
{
    internal static class AttachCourseMappers
    {
        public static Course MapToEntity(this AttachCourseCommand command)
            => Course.Create(
                command.Id,
                command.Title,
                command.Description,
                command.DificultLevel,
                command.Status
                );
    }
}