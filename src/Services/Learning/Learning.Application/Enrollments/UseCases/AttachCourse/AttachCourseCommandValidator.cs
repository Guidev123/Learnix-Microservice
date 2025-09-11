using FluentValidation;

namespace Learning.Application.Enrollments.UseCases.AttachCourse
{
    internal sealed class AttachCourseCommandValidator : AbstractValidator<AttachCourseCommand>
    {
        public AttachCourseCommandValidator()
        {
        }
    }
}