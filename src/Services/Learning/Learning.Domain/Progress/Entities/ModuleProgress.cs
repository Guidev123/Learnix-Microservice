using Learning.Domain.Progress.Enumerators;
using Learning.Domain.Progress.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Progress.Entities
{
    public sealed class ModuleProgress : Entity
    {
        private readonly List<LessonProgress> _lessonsProgress = [];

        private ModuleProgress(Guid moduleId, Guid courseProgressId)
        {
            ModuleId = moduleId;
            CourseProgressId = courseProgressId;
            Status = ModuleStatusEnum.NotStarted;
            Validate();
        }

        private ModuleProgress()
        { }

        public Guid ModuleId { get; private set; }
        public Guid CourseProgressId { get; private set; }
        public ModuleStatusEnum Status { get; private set; }
        public decimal CompletionPercentage { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public IReadOnlyCollection<LessonProgress> LessonsProgress => _lessonsProgress.AsReadOnly();

        public static ModuleProgress Create(Guid moduleId, Guid courseProgressId)
        {
            var moduleProgress = new ModuleProgress(moduleId, courseProgressId);

            return moduleProgress;
        }

        internal void UpdateProgress(List<LessonProgress> moduleLessons)
        {
            if (moduleLessons.Count == 0)
            {
                CompletionPercentage = 0;
                Status = ModuleStatusEnum.NotStarted;
                return;
            }

            if (moduleLessons.Any(l => l.Status == LessonStatusEnum.InProgress) && Status == ModuleStatusEnum.NotStarted)
            {
                Status = ModuleStatusEnum.InProgress;
                StartedAt = DateTime.UtcNow;
            }

            CompletionPercentage = moduleLessons.Average(l => l.CompletionPercentage);

            if (moduleLessons.All(l => l.Status == LessonStatusEnum.Completed) && CompletionPercentage >= 100)
            {
                Status = ModuleStatusEnum.Completed;
                CompletedAt = DateTime.UtcNow;
            }
        }

        internal void Start()
        {
            if (Status == ModuleStatusEnum.NotStarted)
            {
                Status = ModuleStatusEnum.InProgress;
                StartedAt = DateTime.UtcNow;
            }
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, ModuleProgressErrors.ModuleIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(CourseProgressId, Guid.Empty, ModuleProgressErrors.CourseProgressIdMustBeNotEmpty.Description);
        }
    }
}