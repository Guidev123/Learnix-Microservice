using FluentValidation;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Decorators;
using Learnix.Commons.Infrastructure.Clock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.MemoryQueue.DependencyInjection;
using MidR.MemoryQueue.Interfaces;
using System.Reflection;

namespace Learnix.Commons.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddApplication(applicationAssembly);

            return services;
        }

        private static IServiceCollection AddApplication(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddMidR(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

            services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator.RequestHandler<,>));
            services.Decorate(typeof(IRequestHandler<,>), typeof(RequestLoggingDecorator.RequestHandler<,>));

            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}