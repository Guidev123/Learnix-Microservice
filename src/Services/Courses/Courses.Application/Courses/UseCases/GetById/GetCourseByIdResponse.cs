using Courses.Domain.Courses.Enumerators;

namespace Courses.Application.Courses.UseCases.GetById
{
    public sealed record GetCourseByIdResponse
    {
        public GetCourseByIdResponse(Guid id, string title, string description, DificultLevelEnum dificultLevel, List<ModuleResponse> modules)
        {
            Id = id;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Modules = modules;
        }

        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DificultLevelEnum DificultLevel { get; init; }
        public List<ModuleResponse> Modules { get; init; }

        public sealed record ModuleResponse(
            Guid Id,
            string Title,
            uint OrderIndex,
            List<LessonResponse> Lessons,
            Guid? NextModuleId,
            Guid? PreviousModuleId
            );

        public sealed record LessonResponse(
            Guid Id,
            string Title,
            string VideoUrl,
            uint OrderIndex,
            Guid? NextLessonId,
            Guid? PreviousLessonId
            );
    }
}