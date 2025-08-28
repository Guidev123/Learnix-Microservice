using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Module : Entity
    {
        private readonly List<Lesson> _lessons = [];

        private Module(Guid courseId, Guid moduleId, string title)
        {
            Id = moduleId;
            CourseId = courseId;
            Title = title;
        }

        private Module()
        { }

        public Guid CourseId { get; private set; }
        public string Title { get; private set; } = null!;
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();
        public uint LessonsQuantity => (uint)_lessons.Count;
        public uint DurationInMinutes => (uint)_lessons.Sum(m => m.DurationInMinutes);

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}