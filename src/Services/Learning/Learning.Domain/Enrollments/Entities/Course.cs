using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Course : Entity
    {
        private Course(Guid id, string title, string description, string dificultLevel, string status)
        {
            Id = id;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Status = status;
            Validate();
        }

        private Course()
        { }

        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string DificultLevel { get; private set; } = null!;
        public string Status { get; private set; } = null!;
        public List<Module> Modules { get; private set; } = [];

        public static Course Create(Guid id, string title, string description, string dificultLevel, string status, List<Module> modules)
        {
            var course = new Course(id, title, description, dificultLevel, status);

            course.AddModule(modules);

            return course;
        }

        private void AddModule(List<Module> modules)
        {
            Modules.AddRange(modules);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, CourseErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(Title, CourseErrors.CourseTitleMustBeProvided.Description);
            AssertionConcern.EnsureNotEmpty(Description, CourseErrors.CourseDescriptionMustBeProvided.Description);
        }
    }
}