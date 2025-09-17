using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Module : Entity
    {
        private Module(Guid id, string title, uint orderIndex, Guid courseId, Guid? nextModuleId, Guid? previousModuleId)
        {
            Id = id;
            Title = title;
            OrderIndex = orderIndex;
            CourseId = courseId;
            NextModuleId = nextModuleId;
            PreviousModuleId = previousModuleId;
            Validate();
        }

        private Module()
        { }

        public string Title { get; private set; } = null!;
        public uint OrderIndex { get; private set; }
        public Guid CourseId { get; private set; }
        public Guid? NextModuleId { get; private set; }
        public Guid? PreviousModuleId { get; private set; }
        public List<Lesson> Lessons { get; private set; } = [];

        public static Module Create(Guid id, string title, uint orderIndex, Guid courseId, Guid? nextModuleId, Guid? previousModuleId, List<Lesson> lessons)
        {
            var module = new Module(id, title, orderIndex, courseId, nextModuleId, previousModuleId);

            module.AddLessonsRange(lessons);

            return module;
        }

        private void AddLessonsRange(List<Lesson> lessons)
        {
            Lessons.AddRange(lessons);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, ModuleErrors.ModuleIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(Title, CourseErrors.CourseTitleMustBeProvided.Description);
        }
    }
}