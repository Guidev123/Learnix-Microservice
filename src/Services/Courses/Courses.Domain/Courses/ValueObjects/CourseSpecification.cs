using Courses.Domain.Courses.Errors;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Courses.Domain.Courses.ValueObjects
{
    public sealed record CourseSpecification : ValueObject
    {
        public const int MaxTitleLength = 100;
        public const int MinTitleLength = 3;
        public const int MaxDescriptionLength = 500;
        public const int MinDescriptionLength = 10;

        private CourseSpecification(string title, string description)
        {
            Title = title;
            Description = description;
            Validate();
        }

        private CourseSpecification()
        { }

        public string Title { get; } = null!;
        public string Description { get; } = null!;

        public static implicit operator CourseSpecification((string title, string description) specification)
            => new(specification.title, specification.description);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Title, CourseErrors.TitleMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(Description, CourseErrors.DescriptionMustBeNotEmpty.Description);
            AssertionConcern.EnsureInRange(Title.Length, MinTitleLength, MaxTitleLength, CourseErrors.TitleMustBeInRange(MinTitleLength, MaxTitleLength).Description);
            AssertionConcern.EnsureInRange(Description.Length, MinDescriptionLength, MaxDescriptionLength, CourseErrors.DescriptionMustBeInRange(MinDescriptionLength, MaxDescriptionLength).Description);
        }
    }
}