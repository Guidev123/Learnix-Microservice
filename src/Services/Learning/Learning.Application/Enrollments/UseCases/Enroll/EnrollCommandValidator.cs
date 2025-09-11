using FluentValidation;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Progress.Errors;

namespace Learning.Application.Enrollments.UseCases.Enroll
{
    internal sealed class EnrollCommandValidator : AbstractValidator<EnrollCommand>
    {
        public EnrollCommandValidator()
        {
            RuleFor(e => e.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(CourseProgressErrors.CourseIdMustBeNotEmpty.Description);

            RuleFor(e => e.StudentId)
                .NotEqual(Guid.Empty)
                .WithMessage(EnrollmentErrors.StudentIdMustBeNotEmpty.Description);
        }
    }
}