using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.ValueObjects
{
    public sealed record Email : ValueObject
    {
        private Email(string address)
        {
            Address = address;
            Validate();
        }

        private Email()
        { }

        public string Address { get; } = null!;

        public static implicit operator Email(string address) => new(address);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Address, ValueObjectErrors.EmailMustBeNotEmpty.Description);
            AssertionConcern.EnsureMatchesPattern(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", Address, ValueObjectErrors.EmailMustBeValid.Description);
        }
    }
}