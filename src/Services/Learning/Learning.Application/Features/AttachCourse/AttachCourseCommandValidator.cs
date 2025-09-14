using FluentValidation;

namespace Learning.Application.Features.AttachCourse
{
    internal sealed class AttachCourseCommandValidator : AbstractValidator<AttachCourseCommand>
    {
        public AttachCourseCommandValidator()
        {
        }
    }
}