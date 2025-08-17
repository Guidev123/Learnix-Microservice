using Courses.Domain.Courses.Enumerators;

namespace Courses.Application.CoursesContent.UseCases.GetContent
{
    public sealed class CourseContentResponse
    {
        public CourseContentResponse(Guid id, string title, string description, DificultLevelEnum dificultLevel, List<ModuleResponse> modules)
        {
            Id = id;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Modules = modules;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DificultLevelEnum DificultLevel { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }
}