using Courses.Domain.Courses.Errors;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Courses.Domain.Courses.ValueObjects
{
    public sealed record Video : ValueObject
    {
        public const int MaxUrlLength = 200;
        public const int MaxThumbnailUrlLength = 200;

        private Video(string url, string thumbnailUrl)
        {
            Url = url;
            ThumbnailUrl = thumbnailUrl;
            Validate();
        }

        private Video()
        { }

        public string Url { get; } = null!;
        public string ThumbnailUrl { get; } = null!;

        public static implicit operator Video((string url, string thumbnailUrl) video)
            => new(video.url, video.thumbnailUrl);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Url, LessonErrors.UrlMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(ThumbnailUrl, LessonErrors.ThumbnailUrlMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(Uri.TryCreate(Url, UriKind.Absolute, out _), LessonErrors.UrlMustBeValid.Description);
            AssertionConcern.EnsureTrue(Uri.TryCreate(Url, UriKind.Absolute, out _), LessonErrors.UrlMustBeValid.Description);
        }
    }
}