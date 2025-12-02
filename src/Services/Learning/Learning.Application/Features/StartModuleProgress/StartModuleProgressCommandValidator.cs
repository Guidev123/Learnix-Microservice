using FluentValidation;
using Learning.Domain.Progress.Errors;

namespace Learning.Application.Features.StartModuleProgress
{
    internal sealed class StartModuleProgressCommandValidator : AbstractValidator<StartModuleProgressCommand>
    {
        public StartModuleProgressCommandValidator()
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