using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public const int CurrencyCodeLength = 3;
        public const string CurrencyCodePattern = @"^[A-Z]{3}$";
        public const decimal MinimumAmount = 0;

        public Money(decimal price, string currency)
        {
            Amount = price;
            Currency = currency;
            Validate();
        }

        private Money()
        { }

        public decimal Amount { get; }
        public string Currency { get; } = string.Empty;

        public static implicit operator Money((decimal price, string currency) value)
            => new(value.price, value.currency);

        public override string ToString() => $"{Amount} {Currency}";

        protected override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Amount, MinimumAmount, ValueObjectErrors.AmountMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureNotEmpty(Currency, ValueObjectErrors.CurrencyIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Currency, CurrencyCodeLength, CurrencyCodeLength, ValueObjectErrors.CurrencyLengthInvalid.Description);
            AssertionConcern.EnsureMatchesPattern(CurrencyCodePattern, Currency, ValueObjectErrors.CurrencyDoesNotExists.Description);
        }
    }
}