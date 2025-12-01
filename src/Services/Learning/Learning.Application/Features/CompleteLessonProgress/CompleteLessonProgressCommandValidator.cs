using FluentValidation;

namespace Learning.Application.Features.CompleteLessonProgress
{
    internal sealed class CompleteLessonProgressCommandValidator : AbstractValidator<CompleteLessonProgressCommand>
    {
        public CompleteLessonProgressCommandValidator()
        {
        }
    }
}