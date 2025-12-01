using Learning.Domain.Progress.Enumerators;
using Learning.Domain.Progress.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Progress.Entities
{
    public sealed class LessonProgress : Entity
    {
        private LessonProgress(Guid lessonId, Guid moduleId, Guid moduleProgressId)
        {
            LessonId = lessonId;
            ModuleId = moduleId;
            ModuleProgressId = moduleProgressId;
            Status = LessonStatusEnum.NotStarted;
            Validate();
        }

        private LessonProgress()
        { }

        public Guid LessonId { get; private set; }
        public Guid ModuleId { get; private set; }
        public Guid ModuleProgressId { get; private set; }
        public LessonStatusEnum Status { get; private set; }
        public decimal CompletionPercentage { get; private set; }
        public uint MinutesWatched { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public static LessonProgress Create(Guid lessonId, Guid moduleId, Guid moduleProgressId)
        {
            var lessonProgress = new LessonProgress(lessonId, moduleId, moduleProgressId);

            return lessonProgress;
        }

        internal void Start()
        {
            if (Status == LessonStatusEnum.NotStarted)
            {
                Status = LessonStatusEnum.InProgress;
                StartedAt = DateTime.UtcNow;
            }
        }

        internal void UpdateProgress(uint minutesWatched, uint totalLessonDuration)
        {
            MinutesWatched = minutesWatched;

            if (Status == LessonStatusEnum.NotStarted)
            {
                Status = LessonStatusEnum.InProgress;
                StartedAt = DateTime.UtcNow;
            }

            CompletionPercentage = totalLessonDuration > 0
                ? Math.Min(100, (decimal)minutesWatched / totalLessonDuration * 100)
                : 0;

            if (CompletionPercentage >= 80 && Status != LessonStatusEnum.Completed)
            {
                Status = LessonStatusEnum.Completed;
                CompletedAt = DateTime.UtcNow;
            }
        }

        internal void Complete()
        {
            Status = LessonStatusEnum.Completed;
            CompletionPercentage = 100;
            CompletedAt = DateTime.UtcNow;

            if (StartedAt is null)
            {
                StartedAt = DateTime.UtcNow;
            }
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(LessonId, Guid.Empty, LessonProgressErrors.LessonIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, LessonProgressErrors.ModuleIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(ModuleProgressId, Guid.Empty, LessonProgressErrors.ModuleProgressIdMustBeNotEmpty.Description);
        }
    }
}