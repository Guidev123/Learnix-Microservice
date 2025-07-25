using FluentValidation;
using Learnix.Commons.Domain.ValueObjects;
using System.Text.RegularExpressions;
using Users.Domain.Errors;

namespace Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private const int MaxPasswordLength = 8;

        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage(ValueObjectErrors.FullNameMustBeNotEmpty.Description)
                .Must(VerifyFullName)
                .WithMessage(ValueObjectErrors.FullNameMustContainFirstNameAndLastName.Description)
                .Must(VerifyNameLength)
                .WithMessage(ValueObjectErrors.FullNameIsOutOfRange.Description);

            RuleFor(c => c.Email)
                .MaximumLength(Email.MaxEmailLength)
                .WithMessage(ValueObjectErrors.EmailOutOfRange.Description)
                .NotEmpty()
                .WithMessage(ValueObjectErrors.EmailMustBeNotEmpty.Description)
                .EmailAddress()
                .WithMessage(ValueObjectErrors.EmailMustBeValid.Description)
                .Must(Email.IsEmailValid)
                .WithMessage(ValueObjectErrors.EmailMustBeValid.Description);

            RuleFor(c => c.BirthDate)
                .NotEmpty()
                .WithMessage(ValueObjectErrors.BirthDateCannotBeEmpty.Description)
                .Must(Age.BeAtLeastMinAgeYearsOld)
                .WithMessage(ValueObjectErrors.AgeOutOfRange.Description);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(UserErrors.PasswordIsRequired.Description)
                .MinimumLength(MaxPasswordLength).WithMessage(UserErrors.PasswordTooShort.Description)
                .Must(HasUpperCase).WithMessage(UserErrors.PasswordMissingUpperCase.Description)
                .Must(HasLowerCase).WithMessage(UserErrors.PasswordMissingLowerCase.Description)
                .Must(HasDigit).WithMessage(UserErrors.PasswordMissingDigit.Description)
                .Must(HasSpecialCharacter).WithMessage(UserErrors.PasswordMissingSpecialChar.Description);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(UserErrors.PasswordsDoNotMatch.Description)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .NotEmpty().WithMessage(UserErrors.PasswordIsRequired.Description)
                .MinimumLength(MaxPasswordLength).WithMessage(UserErrors.PasswordTooShort.Description);
        }

        private static bool VerifyFullName(string fullName)
        {
            var name = Name.GetFirstAndLastName(fullName);
            return name.Length == 2;
        }

        private static bool VerifyNameLength(string fullName)
        {
            var firstAndLastName = Name.GetFirstAndLastName(fullName);

            if (firstAndLastName.Length != 2)
            {
                return false;
            }

            var firstName = firstAndLastName[0];
            var lastName = firstAndLastName[1];

            var isFirstNameValid = firstName.Length >= Name.NameMinLength || firstName.Length <= Name.NameMaxLength;
            var isLastNameValid = lastName.Length >= Name.NameMinLength || lastName.Length <= Name.NameMaxLength;

            return isFirstNameValid && isLastNameValid;
        }

        private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

        private static bool HasLowerCase(string password) => password.Any(char.IsLower);

        private static bool HasDigit(string password) => password.Any(char.IsDigit);

        private static bool HasSpecialCharacter(string password)
        {
            return new Regex(@"[!@#$%^&*(),.?""{}|<>]").IsMatch(password);
        }
    }
}