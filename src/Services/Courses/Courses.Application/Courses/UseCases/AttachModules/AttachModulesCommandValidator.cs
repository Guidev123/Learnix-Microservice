using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Errors;
using FluentValidation;

namespace Courses.Application.Courses.UseCases.AttachModules
{
    internal sealed class AttachModulesCommandValidator : AbstractValidator<AttachModulesCommand>
    {
        public AttachModulesCommandValidator()
        {
            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage(ModuleErrors.CourseIdMustBeNotEmpty.Description);

            RuleFor(c => c.Modules)
                .NotNull()
                .NotEmpty();

            RuleForEach(c => c.Modules)
                .ChildRules(module =>
                {
                    module.RuleFor(m => m.Title)
                        .NotEmpty()
                        .WithMessage(ModuleErrors.TitleMustBeNotEmpty.Description);

                    module.RuleFor(m => m.Title)
                        .Length(Module.MinTitleLength, Module.MaxTitleLength)
                        .WithMessage(ModuleErrors.TitleMustBeInRange(
                            Module.MinTitleLength,
                            Module.MaxTitleLength
                        ).Description);
                });
        }
    }
}