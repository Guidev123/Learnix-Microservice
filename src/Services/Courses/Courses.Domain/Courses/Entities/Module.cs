using Courses.Domain.Courses.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Courses.Domain.Courses.Entities
{
    public sealed class Module : Entity
    {
        public const int MaxTitleLength = 100;
        public const int MinTitleLength = 3;

        private readonly List<Lesson> _lessons = [];

        private Module(string title, Guid courseId)
        {
            Title = title;
            CourseId = courseId;
            Validate();
        }

        private Module()
        { }

        public string Title { get; private set; } = null!;
        public Guid CourseId { get; private set; }
        public uint OrderIndex { get; private set; }
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();
        public uint DurationInHours => (uint)Math.Ceiling(_lessons.Sum(l => l.DurationInMinutes) / 60.0);
        public uint TotalLessons => (uint)_lessons.Count;

        public static Module Create(string title, Guid courseId)
        {
            var module = new Module(title, courseId);

            return module;
        }

        internal void AddLesson(Lesson lesson)
        {
            lesson.SetOrderIndex((uint)_lessons.Count);
            _lessons.Add(lesson);
        }

        internal void SetOrderIndex(uint orderIndex) => OrderIndex = orderIndex;

        internal Lesson? GetNextLesson(Lesson lesson)
            => _lessons.Where(l => l.OrderIndex > lesson.OrderIndex).OrderBy(l => l.OrderIndex).FirstOrDefault();

        internal Lesson? GetPreviousLesson(Lesson lesson)
            => _lessons.Where(l => l.OrderIndex < lesson.OrderIndex).OrderByDescending(l => l.OrderIndex).FirstOrDefault();

        internal Lesson? GetLessonById(Guid lessonId) => _lessons.FirstOrDefault(l => l.Id == lessonId);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Title, ModuleErrors.TitleMustBeNotEmpty.Description);
            AssertionConcern.EnsureInRange(Title.Length, MinTitleLength, MaxTitleLength, ModuleErrors.TitleMustBeInRange(MinTitleLength, MaxTitleLength).Description);
        }
    }
}