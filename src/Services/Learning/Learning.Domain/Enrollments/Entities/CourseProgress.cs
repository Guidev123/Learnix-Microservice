using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.ValueObjects;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class CourseProgress : Entity
    {
        private CourseProgress(Guid courseId, DateTime startedAt, List<ModuleProgress> modules)
        {
            CourseId = courseId;
            ProgressDateRange = startedAt;
            Status = CourseProgressStatusEnum.InProgress;
            _modules = modules;
            Validate();
        }

        private CourseProgress()
        { }

        public Guid CourseId { get; }
        public CourseProgressStatusEnum Status { get; private set; }
        public CourseProgressDateRange ProgressDateRange { get; private set; } = null!;

        private readonly List<ModuleProgress> _modules = [];
        public IReadOnlyCollection<ModuleProgress> Modules => _modules.AsReadOnly();

        public static CourseProgress Create(Guid courseId, DateTime startedAt, List<ModuleProgress> modules)
        {
            var courseProgress = new CourseProgress(courseId, startedAt, modules);

            return courseProgress;
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
                ?? throw new DomainException(ModuleProgressErrors.NotFound(moduleId).Description);

            module.StartModule();
        }

        internal void CompleteModule(Guid moduleId, Guid enrollmentId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleProgressErrors.NotFound(moduleId).Description);

            module.CompleteModule(enrollmentId);
        }

        internal void CompleteLesson(Guid moduleId, Guid lessonId, Guid enrollmentId)
        {
            var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                ?? throw new DomainException(ModuleProgressErrors.NotFound(moduleId).Description);

            module.CompleteLesson(lessonId, enrollmentId);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, CourseProgressErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(ProgressDateRange, CourseProgressErrors.ProgressDateRangeMustBeNotNull.Description);
            AssertionConcern.EnsureFalse(Modules.Count == 0, CourseProgressErrors.ModulesMustNotBeEmpty.Description);
        }
    }
}