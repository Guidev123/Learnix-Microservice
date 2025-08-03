using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.ValueObjects;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Course : Entity
    {
        private Course(Guid id, Guid enrollmentId, DateTime startedAt, List<Module> modules)
        {
            Id = id;
            EnrollmentId = enrollmentId;
            ProgressDateRange = startedAt;
            Status = CourseProgressStatusEnum.InProgress;
            _modules = modules;
            Validate();
        }

        private Course()
        { }

        public Guid EnrollmentId { get; }
        public CourseProgressStatusEnum Status { get; private set; }
        public ProgressDateRange ProgressDateRange { get; private set; } = null!;

        private readonly List<Module> _modules = [];
        public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();

        public static Course Create(Guid id, Guid enrollmentId, DateTime startedAt, List<Module> modules)
        {
            var course = new Course(id, enrollmentId, startedAt, modules);

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

        internal void CompleteModule(Guid moduleId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleErrors.NotFound(moduleId).Description);

            module.CompleteModule();
        }

        internal void CompleteLesson(Guid moduleId, Guid lessonId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleErrors.NotFound(moduleId).Description);

            module.CompleteLesson(lessonId);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(EnrollmentId, Guid.Empty, CourseErrors.EnrollmentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, CourseErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(ProgressDateRange, CourseErrors.ProgressDateRangeMustBeNotNull.Description);
            AssertionConcern.EnsureFalse(Modules.Count == 0, CourseErrors.ModulesMustNotBeEmpty.Description);
        }
    }
}