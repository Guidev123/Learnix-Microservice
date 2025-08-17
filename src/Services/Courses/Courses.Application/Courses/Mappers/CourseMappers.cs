using Courses.Application.Courses.UseCases.GetContent;
using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Contracts.Courses.IntegrationEvents;
using static Courses.Application.Courses.UseCases.GetContent.GetCourseContentResponse;

namespace Courses.Application.Courses.Mappers
{
    internal static class CourseMappers
    {
        public static GetCourseContentResponse MapFromEntity(this Course course)
        {
            return new GetCourseContentResponse(
                course.Id,
                course.Specification.Title,
                course.Specification.Description,
                course.DificultLevel,
                course.Modules.Select(x => x.MapFromEntity(course)).ToList());
        }

        public static ModuleResponse MapFromEntity(this Module module, Course course)
        {
            return new ModuleResponse(
                module.Id,
                module.Title,
                module.OrderIndex,
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
                course.GetNextLesson(module, lesson)?.Id,
                course.GetPreviousLesson(module, lesson)?.Id
                );
        }

        public static CourseCreatedIntegrationEvent.ModuleResponse MapFromEntityToIntegrationEvent(this Module module, Course course)
        {
            return new CourseCreatedIntegrationEvent.ModuleResponse(
                module.Id,
                module.Title,
                module.OrderIndex,
                [.. module.Lessons.Select(x => x.MapFromEntityToIntegrationEvent(course))],
                course.GetNextModule(module)?.Id,
                course.GetPreviousModule(module)?.Id);
        }

        public static CourseCreatedIntegrationEvent.LessonResponse MapFromEntityToIntegrationEvent(this Lesson lesson, Course course)
        {
            var module = course.Modules.FirstOrDefault(m => m.Id == lesson.ModuleId)
                ?? throw new LearnixException(nameof(Course), ModuleErrors.NotFound(lesson.ModuleId));

            return new CourseCreatedIntegrationEvent.LessonResponse(
                lesson.Id,
                lesson.Title,
                lesson.Video.Url,
                lesson.OrderIndex,
                course.GetNextLesson(module, lesson)?.Id,
                course.GetPreviousLesson(module, lesson)?.Id
                );
        }
    }
}