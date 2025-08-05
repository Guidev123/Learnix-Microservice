using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Enrollments.Enumerators;
using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Module : Entity
    {
        public const int DefaultTotalLessons = 0;
        public const int MinimumCompletedLessons = 1;
        public const int MinimumTotalLessons = 1;

        private Module(Guid id, Guid courseId, List<Lesson> lessons)
        {
            Id = id;
            CourseId = courseId;
            Status = ModuleProgressEnum.NotStarted;
            _lessons = lessons;
            TotalLessons = _lessons.Count;
            Validate();
        }

        private Module()
        { }

        public Guid CourseId { get; }
        public int TotalLessons { get; private set; }
        public int CompletedLessons { get; private set; }
        public double CompletionPercentage { get; private set; }
        public ModuleProgressEnum Status { get; private set; }

        private readonly List<Lesson> _lessons = [];
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

        public static Module Create(Guid id, Guid courseId, List<Lesson> lessons)
        {
            var module = new Module(id, courseId, lessons);

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

                AddDomainEvent(new ModuleCompletedDomainEvent
                {
                    ModuleId = Id,
                    CourseId = CourseId,
                    AggregateId = enrollmentId,
                    Messagetype = nameof(ModuleCompletedDomainEvent)
                });
            }
        }

        internal void GetCompletionPercentage()
        {
            CompletionPercentage = (double)CompletedLessons / TotalLessons * 100;
        }

        internal void CompleteLesson(Guid lessonId, Guid enrollmentId)
        {
            var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId)
                ?? throw new DomainException(ModuleErrors.NotFound(lessonId).Description);

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
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, ModuleErrors.CourseIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureGreaterThanOrEqual(TotalLessons, MinimumTotalLessons, ModuleErrors.TotalLessonsMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureFalse(Lessons.Count == 0, ModuleErrors.LessonsMustNotBeEmpty.Description);
        }
    }
}