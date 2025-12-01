using FluentValidation;

namespace Learning.Application.Features.UpdateLessonProgress
{
    internal sealed class UpdateLessonProgressCommandValidator : AbstractValidator<UpdateLessonProgressCommand>
    {
        public UpdateLessonProgressCommandValidator()
        {
        }
    }
}