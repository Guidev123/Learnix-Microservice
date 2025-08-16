using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class LessonProgress : Entity
    {
        private LessonProgress(Guid lessonId, Guid moduleId)
        {
            LessonId = lessonId;
            ModuleId = moduleId;
            IsCompleted = false;
            Validate();
        }

        private LessonProgress()
        { }

        public Guid LessonId { get; private set; }
        public Guid ModuleId { get; private set; }
        public bool IsCompleted { get; private set; }

        public static LessonProgress Create(Guid lessonId, Guid moduleId)
        {
            var lesson = new LessonProgress(lessonId, moduleId);

            return lesson;
        }

        internal void CompleteLesson(Guid enrollmentId)
        {
            if (IsCompleted) return;
            IsCompleted = true;

            AddDomainEvent(new LessonCompletedDomainEvent(LessonId, ModuleId, enrollmentId));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(LessonId, Guid.Empty, LessonProgressErrors.ModuleIdMustBeValid.Description);
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, LessonProgressErrors.ModuleIdMustBeNotEmpty.Description);
        }
    }
}