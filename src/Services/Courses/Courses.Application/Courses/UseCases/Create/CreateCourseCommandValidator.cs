using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.ValueObjects;
using FluentValidation;

namespace Courses.Application.Courses.UseCases.Create
{
    internal sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(c => c.Title)
                .Length(CourseSpecification.MinTitleLength, CourseSpecification.MaxTitleLength)
                .WithMessage(CourseErrors.TitleMustBeInRange(
                    CourseSpecification.MinTitleLength,
                    CourseSpecification.MaxTitleLength
                ).Description);

            RuleFor(c => c.Description)
                .Length(CourseSpecification.MinTitleLength, CourseSpecification.MaxDescriptionLength)
                .WithMessage(CourseErrors.DescriptionMustBeInRange(
                    CourseSpecification.MinDescriptionLength,
                    CourseSpecification.MaxDescriptionLength
                ).Description);

            RuleFor(c => c.DificultLevel)
                .IsInEnum()
                .WithMessage(
                    CourseErrors.DificultLevelMustBeValid.Description
                );

            RuleFor(c => c.CategoryId)
                .NotEqual(Guid.Empty)
                .WithMessage(
                    CourseErrors.CategoryIdMustBeNotEmpty.Description
                );
        }
    }
}