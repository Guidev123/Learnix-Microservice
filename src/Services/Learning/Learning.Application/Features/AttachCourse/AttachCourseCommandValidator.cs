using FluentValidation;
using Learning.Domain.Enrollments.Errors;

namespace Learning.Application.Features.AttachCourse
{
    internal sealed class AttachCourseCommandValidator : AbstractValidator<AttachCourseCommand>
    {
        public AttachCourseCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage(CourseErrors.CourseIdMustBeNotEmpty.Description);

            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage(CourseErrors.CourseTitleMustBeProvided.Description);

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(CourseErrors.CourseDescriptionMustBeProvided.Description);

            RuleFor(c => c.Status)
                .NotEmpty()
                .WithMessage(CourseErrors.CourseStatusMustBeProvided.Description);

            RuleFor(c => c.DificultLevel)
                .NotEmpty()
                .WithMessage(CourseErrors.CourseDifficultLevelMustBeProvided.Description);
        }
    }
}