using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using FluentValidation;

namespace Courses.Application.Courses.UseCases.AttachLessonsToModule
{
    internal sealed class AttachLessonsToModuleCommandValidator : AbstractValidator<AttachLessonsToModuleCommand>
    {
        public AttachLessonsToModuleCommandValidator()
        {
            RuleFor(x => x.ModuleId)
                .NotEqual(Guid.Empty)
                .WithMessage(LessonErrors.ModuleIdMustBeNotEmpty.Description);

            RuleFor(x => x.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(ModuleErrors.CourseIdMustBeNotEmpty.Description);

            RuleForEach(l => l.Lessons)
                .ChildRules(lesson =>
                {
                    lesson.RuleFor(x => x.Title)
                        .Length(Lesson.MinTitleLength, Lesson.MaxTitleLength)
                        .WithMessage(LessonErrors.TitleMustBeInRange(Lesson.MinTitleLength, Lesson.MaxTitleLength).Description)
                        .NotEmpty()
                        .WithMessage(LessonErrors.TitleMustBeNotEmpty.Description);

                    lesson.RuleFor(x => x.DurationInMinutes)
                        .GreaterThan(0)
                        .WithMessage(LessonErrors.DurationInMinutesShouldBeGreaterThanZero.Description);

                    lesson.RuleFor(x => x.VideoUrl)
                        .Must(IsValidUrl)
                        .WithMessage(LessonErrors.UrlMustBeValid.Description);

                    lesson.RuleFor(x => x.VideoThumbnailUrl)
                        .Must(IsValidUrl)
                        .WithMessage(LessonErrors.ThumbnailUrlMustBeValid.Description);
                });
        }

        private static bool IsValidUrl(string url)
            => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}