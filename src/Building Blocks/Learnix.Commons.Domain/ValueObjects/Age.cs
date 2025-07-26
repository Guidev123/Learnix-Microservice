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

        public static bool BeAtLeastMinAgeYearsOld(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (age >= MaxAge)
            {
                return false;
            }

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= MinAge;
        }
        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(BeAtLeastMinAgeYearsOld(BirthDate), ValueObjectErrors.AgeOutOfRange.Description);
        }
    }
}