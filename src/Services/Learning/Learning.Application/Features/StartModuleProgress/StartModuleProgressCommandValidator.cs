using FluentValidation;

namespace Learning.Application.Features.StartModuleProgress
{
    internal sealed class StartModuleProgressCommandValidator : AbstractValidator<StartModuleProgressCommand>
    {
        public StartModuleProgressCommandValidator()
        {
        }
    }
}