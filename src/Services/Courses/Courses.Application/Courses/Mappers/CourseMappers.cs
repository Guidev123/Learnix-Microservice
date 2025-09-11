using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using Learnix.Commons.Application.Exceptions;
using static Learnix.Commons.Contracts.Courses.IntegrationEvents.CourseAttachedIntegrationEvent;

namespace Courses.Application.Courses.Mappers
{
    internal static class CourseMappers
    {
        public static ModuleResponse MapFromEntity(this Module module, Course course)
        {
            return new ModuleResponse(
                module.Id,
                module.Title,
                module.OrderIndex,
                module.CourseId,
                [.. module.Lessons.Select(x => x.MapFromEntity(course))],
                course.GetNextModule(module)?.Id,
                course.GetPreviousModule(module)?.Id);
        }

        public static LessonResponse MapFromEntity(this Lesson lesson, Course course)
        {
            var module = course.Modules.FirstOrDefault(m => m.Id == lesson.ModuleId)
                ?? throw new LearnixException(nameof(Course), ModuleErrors.NotFound(lesson.ModuleId));

            return new LessonResponse(
                lesson.Id,
                lesson.Title,
                lesson.Video.Url,
                lesson.OrderIndex,
                lesson.ModuleId,
                course.GetNextLesson(module, lesson)?.Id,
                course.GetPreviousLesson(module, lesson)?.Id
                );
        }

        public static ModuleResponse MapFromEntityToIntegrationEvent(this Module module, Course course)
        {
            return new ModuleResponse(
                module.Id,
                module.Title,
                module.OrderIndex,
                module.CourseId,
                [.. module.Lessons.Select(x => x.MapFromEntityToIntegrationEvent(course))],
                course.GetNextModule(module)?.Id,
                course.GetPreviousModule(module)?.Id);
        }

        public static LessonResponse MapFromEntityToIntegrationEvent(this Lesson lesson, Course course)
        {
            var module = course.Modules.FirstOrDefault(m => m.Id == lesson.ModuleId)
                ?? throw new LearnixException(nameof(Course), ModuleErrors.NotFound(lesson.ModuleId));

            return new LessonResponse(
                lesson.Id,
                lesson.Title,
                lesson.Video.Url,
                lesson.OrderIndex,
                lesson.ModuleId,
                course.GetNextLesson(module, lesson)?.Id,
                course.GetPreviousLesson(module, lesson)?.Id
                );
        }
    }
}