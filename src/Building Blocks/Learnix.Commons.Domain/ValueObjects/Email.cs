using Learnix.Commons.Domain.DomainObjects;
using System.Text.RegularExpressions;

namespace Learnix.Commons.Domain.ValueObjects
{
    public sealed record Email : ValueObject
    {
        public const int MaxEmailLength = 160;

        private Email(string address)
        {
            Address = address;
            Validate();
        }

        private Email()
        { }

        public string Address { get; } = null!;

        public static implicit operator Email(string address) => new(address);

        public static bool IsEmailValid(string address) => Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        protected override void Validate()
        {
            AssertionConcern.EnsureMaxLength(Address, MaxEmailLength, ValueObjectErrors.EmailOutOfRange.Description);
            AssertionConcern.EnsureNotEmpty(Address, ValueObjectErrors.EmailMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(IsEmailValid(Address), ValueObjectErrors.EmailMustBeValid.Description);
        }
    }
}