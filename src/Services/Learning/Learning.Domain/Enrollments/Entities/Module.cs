using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Module : Entity
    {
        private readonly List<Lesson> _lessons = [];

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
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

        public static Module Create(Guid id, string title, uint orderIndex, Guid courseId, Guid? nextModuleId, Guid? previousModuleId)
        {
            var module = new Module(id, title, orderIndex, courseId, nextModuleId, previousModuleId);

            return module;
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, ModuleErrors.ModuleIdMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(Title, CourseErrors.CourseTitleMustBeProvided.Description);
        }
    }
}