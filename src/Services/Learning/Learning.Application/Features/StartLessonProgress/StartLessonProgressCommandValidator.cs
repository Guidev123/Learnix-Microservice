using FluentValidation;
using Learning.Domain.Progress.Errors;

namespace Learning.Application.Features.StartLessonProgress
{
    internal sealed class StartLessonProgressCommandValidator : AbstractValidator<StartLessonProgressCommand>
    {
        public StartLessonProgressCommandValidator()
        {
            RuleFor(c => c.LessonId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonProgressErrors.LessonIdMustBeNotEmpty.Description);

            RuleFor(c => c.ModuleId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonProgressErrors.ModuleIdMustBeNotEmpty.Description);

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonProgressErrors.CourseIdMustBeNotEmpty.Description);
        }
    }
}