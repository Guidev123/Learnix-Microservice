using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.ValueObjects
{
    public sealed record Age : ValueObject
    {
        public const int MinAge = 16;
        public const int MaxAge = 130;

        private Age(DateTime birthDate)
        {
            BirthDate = birthDate;
            Validate();
        }

        private Age()
        { }

        public DateTime BirthDate { get; }

        public static implicit operator Age(DateTime birthDate) => new(birthDate);

        public override string ToString() => $"{DateTime.Today.Year - BirthDate.Year -
            (DateTime.Today < BirthDate.AddYears(DateTime.Today.Year - BirthDate.Year) ? 1 : 0)}";

        protected override void Validate()
        {
            var today = DateTime.Today;

            AssertionConcern.EnsureTrue(BirthDate <= today, ValueObjectErrors.BirthDateCannotBeInFuture.Description);

            var age = today.Year - BirthDate.Year;
            if (today < BirthDate.AddYears(age)) age--;

            AssertionConcern.EnsureInRange(age, MinAge, MaxAge, ValueObjectErrors.AgeOutOfRange.Description);
        }
    }
}