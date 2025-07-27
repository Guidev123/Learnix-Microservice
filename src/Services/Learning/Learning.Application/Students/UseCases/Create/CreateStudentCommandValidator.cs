using FluentValidation;
using Learning.Domain.Errors;

namespace Learning.Application.Students.UseCases.Create
{
    internal sealed class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .WithMessage(StudentErrors.FirstNameMustBeNotEmpty.Description);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .WithMessage(StudentErrors.LastNameMustBeNotEmpty.Description);

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage(StudentErrors.EmailMustBeNotEmpty.Description)
                .EmailAddress()
                .WithMessage(StudentErrors.EmailIsInvalid.Description);

            RuleFor(c => c.BirthDate)
                .NotEmpty()
                .WithMessage(StudentErrors.BirthDateMustBeNotEmpty.Description);
        }
    }
}