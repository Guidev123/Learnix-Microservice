using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Progress.Enumerators;
using Learning.Domain.Progress.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Progress.Entities
{
    public sealed class CourseProgress : Entity, IAggregateRoot
    {
        private readonly List<ModuleProgress> _moduleProgresses = [];

        private CourseProgress(Guid studentId, Guid enrollmentId, Guid courseId, DateTime startedAt)
        {
            StudentId = studentId;
            EnrollmentId = enrollmentId;
            CourseId = courseId;
            StartedAt = startedAt;
            Status = ProgressStatusEnum.NotStarted;
            Validate();
        }

        private CourseProgress()
        { }

        public Guid StudentId { get; private set; }
        public Guid EnrollmentId { get; private set; }
        public Guid CourseId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public ProgressStatusEnum Status { get; private set; }
        public decimal OverallCompletionPercentage { get; private set; }
        public uint TotalMinutesWatched { get; private set; }

        public IReadOnlyCollection<ModuleProgress> ModulesProgress => _moduleProgresses.AsReadOnly();

        public static CourseProgress Create(Guid studentId, Guid enrollmentId, Guid courseId, DateTime startedAt)
        {
            var progress = new CourseProgress(studentId, enrollmentId, courseId, startedAt);

            return progress;
        }

        public void StartLesson(Guid lessonId, Guid moduleId)
        {
            var moduleProgress = GetModuleProgress(moduleId);
            var lessonProgress = GetLessonProgressOrDefault(moduleProgress, lessonId);

            if (lessonProgress is null)
            {
                lessonProgress = LessonProgress.Create(lessonId, moduleId, moduleProgress.Id);
                moduleProgress.AddLessonProgress(lessonProgress);
            }

            lessonProgress.Start();

            if (Status == ProgressStatusEnum.NotStarted)
            {
                Status = ProgressStatusEnum.InProgress;
            }

            UpdateModuleProgress(moduleId);
            UpdateOverallProgress();

            AddDomainEvent(new LessonStartedDomainEvent(Id, StudentId, lessonId, moduleId, CourseId));
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
            var lessonProgress = GetLessonProgressOrDefault(moduleProgress, lessonId);

            if (lessonProgress is null)
            {
                lessonProgress = LessonProgress.Create(lessonId, moduleId, moduleProgress.Id);
                moduleProgress.AddLessonProgress(lessonProgress);
            }

            var previousMinutes = lessonProgress.MinutesWatched;
            lessonProgress.UpdateProgress(minutesWatched, totalLessonDuration);

            TotalMinutesWatched = TotalMinutesWatched - previousMinutes + minutesWatched;

            UpdateModuleProgress(moduleId);
            UpdateOverallProgress();

            AddDomainEvent(new LessonProgressUpdatedDomainEvent(Id, StudentId, lessonId, moduleId, CourseId, minutesWatched));
        }

        private void UpdateModuleProgress(Guid moduleId)
        {
            var moduleProgress = GetModuleProgressOrDefault(moduleId);

            if (moduleProgress is null)
            {
                moduleProgress = ModuleProgress.Create(moduleId, Id);
                _moduleProgresses.Add(moduleProgress);
            }

            var moduleLessons = moduleProgress.LessonsProgress.Where(lp => lp.ModuleId == moduleId).ToList();
            moduleProgress.UpdateProgress(moduleLessons);
        }

        private void UpdateOverallProgress()
        {
            if (!_moduleProgresses.Any())
            {
                OverallCompletionPercentage = 0;
                return;
            }

            OverallCompletionPercentage = _moduleProgresses.Average(mp => mp.CompletionPercentage);

            if (OverallCompletionPercentage >= 100 && Status != ProgressStatusEnum.Completed)
            {
                Status = ProgressStatusEnum.Completed;
                CompletedAt = DateTime.UtcNow;
                AddDomainEvent(new CourseCompletedDomainEvent(StudentId, CourseId, EnrollmentId, CompletedAt.Value));
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

        private ModuleProgress GetModuleProgress(Guid moduleId)
            => _moduleProgresses.FirstOrDefault(m => m.ModuleId == moduleId)
                ?? throw new DomainException(ModuleProgressErrors.ModuleProgressNotFound.Description);

        private ModuleProgress? GetModuleProgressOrDefault(Guid moduleId)
            => _moduleProgresses.FirstOrDefault(m => m.ModuleId == moduleId);

        private static LessonProgress GetLessonProgress(ModuleProgress moduleProgress, Guid lessonId)
            => moduleProgress.LessonsProgress.FirstOrDefault(l => l.LessonId == lessonId)
                ?? throw new DomainException(LessonProgressErrors.LessonProgressNotFound.Description);

        private static LessonProgress? GetLessonProgressOrDefault(ModuleProgress moduleProgress, Guid lessonId)
            => moduleProgress.LessonsProgress.FirstOrDefault(l => l.LessonId == lessonId);

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(StudentId, Guid.Empty, CourseProgressErrors.StudentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(EnrollmentId, Guid.Empty, CourseProgressErrors.EnrollmentIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureDifferent(CourseId, Guid.Empty, CourseProgressErrors.CourseIdMustBeNotEmpty.Description);
        }
    }
}