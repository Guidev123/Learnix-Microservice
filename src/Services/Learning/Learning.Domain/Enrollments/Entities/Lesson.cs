using Learning.Domain.Enrollments.DomainEvents;
using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Lesson : Entity
    {
        private Lesson(Guid id, Guid moduleId)
        {
            Id = id;
            ModuleId = moduleId;
            IsCompleted = false;
            Validate();
        }

        private Lesson()
        { }

        public Guid ModuleId { get; private set; }
        public bool IsCompleted { get; private set; }

        public static Lesson Create(Guid id, Guid moduleId)
        {
            var lesson = new Lesson(id, moduleId);

            return lesson;
        }

        internal void CompleteLesson()
        {
            if (IsCompleted) return;
            IsCompleted = true;

            AddDomainEvent(new LessonCompletedDomainEvent(Id, ModuleId));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, LessonErrors.ModuleIdMustBeNotEmpty.Description);
        }
    }
}