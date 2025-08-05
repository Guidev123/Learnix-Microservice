using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.ValueObjects;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Course : Entity
    {
        private Course(Guid id, DateTime startedAt, List<Module> modules)
        {
            Id = id;
            ProgressDateRange = startedAt;
            Status = CourseProgressStatusEnum.InProgress;
            _modules = modules;
            Validate();
        }

        private Course()
        { }

        public CourseProgressStatusEnum Status { get; private set; }
        public CourseProgressDateRange ProgressDateRange { get; private set; } = null!;

        private readonly List<Module> _modules = [];
        public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();

        public static Course Create(Guid id, DateTime startedAt, List<Module> modules)
        {
            var course = new Course(id, startedAt, modules);

            return course;
        }

        internal void StartCourse()
        {
            if (Status == CourseProgressStatusEnum.InProgress) return;
            Status = CourseProgressStatusEnum.InProgress;
        }

        internal void CompleteCourse(DateTime completedAt)
        {
            if (Status == CourseProgressStatusEnum.Completed) return;
            Status = CourseProgressStatusEnum.Completed;
            ProgressDateRange = (ProgressDateRange.StartedAt, completedAt);
        }

        internal void StartModule(Guid moduleId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleErrors.NotFound(moduleId).Description);

            module.StartModule();
        }

        internal void CompleteModule(Guid moduleId, Guid enrollmentId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleErrors.NotFound(moduleId).Description);

            module.CompleteModule(enrollmentId);
        }

        internal void CompleteLesson(Guid moduleId, Guid lessonId, Guid enrollmentId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleErrors.NotFound(moduleId).Description);

            module.CompleteLesson(lessonId, enrollmentId);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, CourseErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(ProgressDateRange, CourseErrors.ProgressDateRangeMustBeNotNull.Description);
            AssertionConcern.EnsureFalse(Modules.Count == 0, CourseErrors.ModulesMustNotBeEmpty.Description);
        }
    }
}