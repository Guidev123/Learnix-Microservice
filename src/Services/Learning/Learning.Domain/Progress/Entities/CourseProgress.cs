using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Progress.Enumerators;
using Learning.Domain.Progress.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Progress.Entities
{
    public sealed class CourseProgress : Entity, IAggregateRoot
    {
        private readonly List<ModuleProgress> _moduleProgresses = [];

        private CourseProgress(Guid studentId, Guid courseId, DateTime startedAt)
        {
            StudentId = studentId;
            CourseId = courseId;
            StartedAt = startedAt;
            Status = ProgressStatusEnum.NotStarted;
            Validate();
        }

        private CourseProgress()
        { }

        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public ProgressStatusEnum Status { get; private set; }
        public decimal OverallCompletionPercentage { get; private set; }
        public uint TotalMinutesWatched { get; private set; }

        public IReadOnlyCollection<ModuleProgress> ModulesProgress => _moduleProgresses.AsReadOnly();

        public static CourseProgress Create(Guid studentId, Guid courseId, DateTime startedAt)
        {
            var progress = new CourseProgress(studentId, courseId, startedAt);

            return progress;
        }

        public void StartLesson(LessonProgress lessonProgress, Guid moduleId)
        {
            lessonProgress.Start();

            if (Status == ProgressStatusEnum.NotStarted)
            {
                Status = ProgressStatusEnum.InProgress;
            }

            UpdateModuleProgress(moduleId);
            UpdateOverallProgress();

            AddDomainEvent(new LessonStartedDomainEvent(Id, StudentId, lessonProgress.Id, moduleId, CourseId));
        }

        public void UpdateModuleProgress(ModuleProgress moduleProgress)
        {
            moduleProgress.Start();
        }

        public void CompleteLesson(Guid lessonId, Guid moduleId, uint lessonDurationInMinutes)
        {
            var lessonProgress = GetLessonProgress(GetModuleProgress(moduleId), lessonId);

            lessonProgress.Complete();
            TotalMinutesWatched += lessonDurationInMinutes;

            UpdateModuleProgress(moduleId);
            UpdateOverallProgress();

            AddDomainEvent(new LessonCompletedDomainEvent(StudentId, lessonId, moduleId, CourseId));
        }

        public void UpdateLessonProgress(Guid lessonId, Guid moduleId, uint minutesWatched, uint totalLessonDuration)
        {
            var moduleProgress = GetModuleProgress(moduleId);
            var lessonProgress = GetLessonProgressOrDefault(moduleProgress, lessonId)
                ?? throw new DomainException(LessonProgressErrors.LessonProgressNotFound.Description);

            var previousMinutes = lessonProgress.MinutesWatched;
            lessonProgress.UpdateProgress(minutesWatched, totalLessonDuration);

            TotalMinutesWatched = TotalMinutesWatched - previousMinutes + minutesWatched;

            UpdateModuleProgress(moduleId);
            UpdateOverallProgress();

            AddDomainEvent(new LessonProgressUpdatedDomainEvent(Id, StudentId, lessonId, moduleId, CourseId, minutesWatched));
        }

        private void UpdateModuleProgress(Guid moduleId)
        {
            var moduleProgress = GetModuleProgressOrDefault(moduleId)
                ?? throw new DomainException(ModuleProgressErrors.NotFound(moduleId).Description);

            var moduleLessons = moduleProgress.LessonsProgress.Where(lp => lp.ModuleId == moduleId).ToList();
            moduleProgress.UpdateProgress(moduleLessons);
        }

        private void UpdateOverallProgress()
        {
            if (_moduleProgresses.Count == 0)
            {
                OverallCompletionPercentage = 0;
                return;
            }

            OverallCompletionPercentage = _moduleProgresses.Average(mp => mp.CompletionPercentage);

            if (OverallCompletionPercentage >= 100 && Status != ProgressStatusEnum.Completed)
            {
                Status = ProgressStatusEnum.Completed;
                CompletedAt = DateTime.UtcNow;
                AddDomainEvent(new CourseCompletedDomainEvent(StudentId, CourseId, CompletedAt.Value));
            }
        }

        public decimal GetModuleCompletionPercentage(Guid moduleId)
        {
            var moduleProgress = _moduleProgresses.FirstOrDefault(mp => mp.ModuleId == moduleId);
            return moduleProgress?.CompletionPercentage ?? 0;
        }

        public decimal GetLessonCompletionPercentage(Guid moduleId, Guid lessonId)
        {
            var lessonProgress = GetModuleProgress(moduleId).LessonsProgress.FirstOrDefault(lp => lp.LessonId == lessonId);
            return lessonProgress?.CompletionPercentage ?? 0;
        }

        public bool IsLessonCompleted(Guid moduleId, Guid lessonId)
        {
            var lessonProgress = GetModuleProgress(moduleId).LessonsProgress.FirstOrDefault(lp => lp.LessonId == lessonId);
            return lessonProgress?.Status == LessonStatusEnum.Completed;
        }

        public bool IsModuleCompleted(Guid moduleId)
        {
            var moduleProgress = _moduleProgresses.FirstOrDefault(mp => mp.ModuleId == moduleId);
            return moduleProgress?.Status == ModuleStatusEnum.Completed;
        }

        public ModuleProgress GetModuleProgress(Guid moduleId)
            => _moduleProgresses.FirstOrDefault(m => m.ModuleId == moduleId)
                ?? throw new DomainException(ModuleProgressErrors.ModuleProgressNotFound.Description);

        public LessonProgress? GetLessonProgressOrDefault(ModuleProgress moduleProgress, Guid lessonId)
            => moduleProgress.LessonsProgress.FirstOrDefault(l => l.LessonId == lessonId);

        public ModuleProgress? GetModuleProgressOrDefault(Guid moduleId)
            => _moduleProgresses.FirstOrDefault(m => m.ModuleId == moduleId);

        private static LessonProgress GetLessonProgress(ModuleProgress moduleProgress, Guid lessonId)
            => moduleProgress.LessonsProgress.FirstOrDefault(l => l.LessonId == lessonId)
                ?? throw new DomainException(LessonProgressErrors.LessonProgressNotFound.Description);

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(StudentId, Guid.Empty, CourseProgressErrors.StudentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, CourseProgressErrors.CourseIdMustBeNotEmpty.Description);
        }
    }
}