using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class ModuleProgress : Entity
    {
        public const int DefaultTotalLessons = 0;
        public const int MinimumCompletedLessons = 1;
        public const int MinimumTotalLessons = 1;

        private ModuleProgress(Guid moduleId, Guid courseId, List<LessonProgress> lessons)
        {
            ModuleId = moduleId;
            CourseId = courseId;
            Status = ModuleProgressEnum.NotStarted;
            _lessons = lessons;
            TotalLessons = _lessons.Count;
            Validate();
        }

        private ModuleProgress()
        { }

        public Guid ModuleId { get; private set; }
        public Guid CourseId { get; }
        public int TotalLessons { get; private set; }
        public int CompletedLessons { get; private set; }
        public double CompletionPercentage { get; private set; }
        public ModuleProgressEnum Status { get; private set; }

        private readonly List<LessonProgress> _lessons = [];
        public IReadOnlyCollection<LessonProgress> Lessons => _lessons.AsReadOnly();

        public static ModuleProgress Create(Guid moduleId, Guid courseId, List<LessonProgress> lessons)
        {
            var module = new ModuleProgress(moduleId, courseId, lessons);

            return module;
        }

        internal void StartModule()
        {
            if (Status == ModuleProgressEnum.Started) return;
            Status = ModuleProgressEnum.Started;
        }

        internal void CompleteModule(Guid enrollmentId)
        {
            if (TotalLessons == CompletedLessons)
            {
                if (Status == ModuleProgressEnum.Completed) return;
                Status = ModuleProgressEnum.Completed;

                AddDomainEvent(new ModuleCompletedDomainEvent(ModuleId, CourseId, enrollmentId));
            }
        }

        internal void GetCompletionPercentage()
        {
            CompletionPercentage = (double)CompletedLessons / TotalLessons * 100;
        }

        internal void CompleteLesson(Guid lessonId, Guid enrollmentId)
        {
            var lesson = _lessons.FirstOrDefault(l => l.LessonId == lessonId)
                ?? throw new DomainException(ModuleProgressErrors.NotFound(lessonId).Description);

            if (lesson.IsCompleted is false)
            {
                lesson.CompleteLesson(enrollmentId);
                CompletedLessons += 1;
                GetCompletionPercentage();
                if (CompletedLessons == TotalLessons)
                {
                    CompleteModule(enrollmentId);
                }
            }
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, ModuleProgressErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, ModuleProgressErrors.ModuleIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureGreaterThanOrEqual(TotalLessons, MinimumTotalLessons, ModuleProgressErrors.TotalLessonsMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureFalse(Lessons.Count == 0, ModuleProgressErrors.LessonsMustNotBeEmpty.Description);
        }
    }
}