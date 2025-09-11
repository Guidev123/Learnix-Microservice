using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Lesson : Entity
    {
        private Lesson(Guid id, string title, string videoUrl, uint orderIndex, Guid moduleId, Guid? nextLessonId, Guid? previousLessonId)
        {
            Id = id;
            Title = title;
            VideoUrl = videoUrl;
            OrderIndex = orderIndex;
            ModuleId = moduleId;
            NextLessonId = nextLessonId;
            PreviousLessonId = previousLessonId;
            Validate();
        }

        private Lesson()
        { }

        public string Title { get; private set; } = null!;
        public string VideoUrl { get; private set; } = null!;
        public uint OrderIndex { get; private set; }
        public Guid ModuleId { get; private set; }
        public Guid? NextLessonId { get; private set; }
        public Guid? PreviousLessonId { get; private set; }

        public static Lesson Create(Guid id, string title, string videoUrl, uint orderIndex, Guid moduleId, Guid? nextLessonId, Guid? previousLessonId)
        {
            var lesson = new Lesson(id, title, videoUrl, orderIndex, moduleId, nextLessonId, previousLessonId);

            return lesson;
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(Id, Guid.Empty, LessonErrors.LessonIdMustBeNotEmpty.Description);
        }
    }
}