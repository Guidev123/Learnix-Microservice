using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Courses.Enumerators;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.ValueObjects;
using Learnix.Commons.Domain.DomainObjects;

namespace Courses.Domain.Courses.Entities
{
    public sealed class Course : Entity, IAggregateRoot
    {
        private readonly List<Module> _modules = [];

        private Course(
            string title,
            string description,
            DificultLevelEnum dificultLevel,
            Guid categoryId)
        {
            Specification = (title, description);
            DificultLevel = dificultLevel;
            CategoryId = categoryId;
            Validate();
        }

        public Course()
        { }

        public CourseSpecification Specification { get; private set; } = null!;
        public DificultLevelEnum DificultLevel { get; private set; }
        public Guid CategoryId { get; private set; }
        public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();
        public uint ModulesQuantity => (uint)_modules.Count;
        public uint DurationInHours => (uint)_modules.Sum(m => m.DurationInHours);

        public void AddModule(Module module)
        {
            module.SetOrderIndex((uint)_modules.Count + 1);
            _modules.Add(module);
        }

        public void AddLessonToModule(Guid moduleId, Lesson lesson)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId);
            AssertionConcern.EnsureNotNull(module, ModuleErrors.NotFound(moduleId).Description);

            module?.AddLesson(lesson);
        }

        public IEnumerable<Module> GetModulesInOrder() => _modules.OrderBy(m => m.OrderIndex);

        public Module? GetModuleById(Guid moduleId) => _modules.FirstOrDefault(m => m.Id == moduleId);

        public Lesson? GetLessonByModuleId(Guid moduleId, Guid lessonId)
            => _modules.FirstOrDefault(m => m.Id == moduleId)?.GetLessonById(lessonId);

        public Module? GetNextModule(Module module)
            => _modules.Where(m => m.OrderIndex > module.OrderIndex).OrderBy(m => m.OrderIndex).FirstOrDefault();

        public Module? GetPreviousModule(Module module)
            => _modules.Where(m => m.OrderIndex < module.OrderIndex).OrderByDescending(m => m.OrderIndex).FirstOrDefault();

        public Lesson? GetNextLesson(Module module, Lesson lesson) => module.GetNextLesson(lesson);

        public Lesson? GetPreviousLesson(Module module, Lesson lesson) => module.GetPreviousLesson(lesson);

        public static Course Create(
            string title,
            string description,
            DificultLevelEnum dificultLevel,
            Guid categoryId)
        {
            var course = new Course(title, description, dificultLevel, categoryId);

            course.AddDomainEvent(new CourseCreatedDomainEvent(course.Id));

            return course;
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Specification, CourseErrors.CourseSpecificationMustBeNotNull.Description);
            AssertionConcern.EnsureTrue(Enum.IsDefined(DificultLevel), CourseErrors.DificultLevelMustBeValid.Description);
            AssertionConcern.EnsureDifferent(CategoryId, Guid.Empty, CourseErrors.CategoryIdMustBeNotEmpty.Description);
        }
    }
}