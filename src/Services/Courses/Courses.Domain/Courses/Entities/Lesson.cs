using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.ValueObjects;
using Learnix.Commons.Domain.DomainObjects;

namespace Courses.Domain.Courses.Entities
{
    public sealed class Lesson : Entity
    {
        public const int MaxTitleLength = 100;
        public const int MinTitleLength = 3;

        private Lesson(
            string title,
            string videoUrl,
            string videoThumbnailUrl,
            uint durationInMinutes,
            Guid moduleId)
        {
            Title = title;
            Video = (videoUrl, videoThumbnailUrl);
            DurationInMinutes = durationInMinutes;
            ModuleId = moduleId;
            Validate();
        }

        private Lesson()
        { }

        public string Title { get; private set; } = null!;
        public Video Video { get; private set; } = null!;
        public Guid ModuleId { get; private set; }
        public uint OrderIndex { get; private set; }
        public uint DurationInMinutes { get; private set; }

        public static Lesson Create(
            string title,
            string videoUrl,
            string videoThumbnailUrl,
            uint durationInMinutes,
            Guid moduleId)
        {
            var lesson = new Lesson(title, videoUrl, videoThumbnailUrl, durationInMinutes, moduleId);

            return lesson;
        }

        internal void SetOrderIndex(uint orderIndex) => OrderIndex = orderIndex;

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Title, LessonErrors.TitleMustBeNotEmpty.Description);
            AssertionConcern.EnsureInRange(Title.Length, MinTitleLength, MaxTitleLength, LessonErrors.TitleMustBeInRange(MinTitleLength, MaxTitleLength).Description);
            AssertionConcern.EnsureDifferent(ModuleId, Guid.Empty, LessonErrors.ModuleIdMustBeNotEmpty.Description);
        }
    }
}