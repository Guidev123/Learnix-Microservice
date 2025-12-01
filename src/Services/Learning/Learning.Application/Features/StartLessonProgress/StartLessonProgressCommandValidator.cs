using FluentValidation;

namespace Learning.Application.Features.StartLessonProgress
{
    internal sealed class StartLessonProgressCommandValidator : AbstractValidator<StartLessonProgressCommand>
    {
        public StartLessonProgressCommandValidator()
        {
        }
    }
}