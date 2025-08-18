using Courses.Domain.Courses.Errors;
using FluentValidation;

namespace Courses.Application.Courses.UseCases.Publish
{
    internal sealed class PublishCourseCommandValidator : AbstractValidator<PublishCourseCommand>
    {
        public PublishCourseCommandValidator()
        {
            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(CourseErrors.CourseIdMustBeNotEmpty.Description);
        }
    }
}