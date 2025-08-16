using Courses.Domain.Courses.Errors;
using Learnix.Commons.Domain.DomainObjects;

namespace Courses.Domain.Courses.Entities
{
    public sealed class Category : Entity
    {
        public const int MaxNameLength = 100;
        public const int MinNameLength = 3;

        private Category(string name)
        {
            Name = name;
            Validate();
        }

        private Category()
        { }

        public string Name { get; private set; } = null!;

        public static Category Create(string name)
        {
            var category = new Category(name);

            return category;
        }

        public void UpdateName(string name)
        {
            Name = name;
            Validate();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, CategoryErrors.NameMustBeNotEmpty.Description);
            AssertionConcern.EnsureInRange(Name.Length, MinNameLength, MaxNameLength, CategoryErrors.NameMustBeInRange(MinNameLength, MaxNameLength).Description);
        }
    }
}