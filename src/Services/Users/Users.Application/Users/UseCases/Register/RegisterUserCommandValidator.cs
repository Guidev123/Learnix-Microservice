using FluentValidation;

namespace Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
        }
    }
}