using FluentValidation;
using Learning.Domain.Progress.Errors;

namespace Learning.Application.Features.UpdateLessonProgress
{
    internal sealed class UpdateLessonProgressCommandValidator : AbstractValidator<UpdateLessonProgressCommand>
    {
        public UpdateLessonProgressCommandValidator()
        {
            RuleFor(c => c.ModuleId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonProgressErrors.ModuleIdMustBeNotEmpty.Description);

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonProgressErrors.CourseIdMustBeNotEmpty.Description);
        }
    }
}