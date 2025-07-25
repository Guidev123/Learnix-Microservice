using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.ValueObjects
{
    public sealed record Name : ValueObject
    {
        public const int NameMaxLength = 50;
        public const int NameMinLength = 2;

        private Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Validate();
        }

        private Name()
        { }

        public string FirstName { get; } = null!;
        public string LastName { get; } = null!;

        public static implicit operator Name(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new DomainException(ValueObjectErrors.FullNameMustBeNotEmpty.Description);
            }

            var parts = GetFirstAndLastName(fullName);

            if (parts.Length != 2)
            {
                throw new DomainException(ValueObjectErrors.FullNameMustContainFirstNameAndLastName.Description);
            }

            return new Name(parts[0], parts[1]);
        }

        public static string[] GetFirstAndLastName(string fullName) => fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(FirstName, ValueObjectErrors.FirstNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureLengthInRange(FirstName, NameMinLength, NameMaxLength, ValueObjectErrors.FirstNameLengthInvalid.Description);

            AssertionConcern.EnsureNotEmpty(LastName, ValueObjectErrors.LastNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureLengthInRange(LastName, NameMinLength, NameMaxLength, ValueObjectErrors.LastNameLengthInvalid.Description);
        }
    }
}