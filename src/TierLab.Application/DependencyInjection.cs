using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TierLab.Application;

/// <summary>
/// Registers all Application layer services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Register all FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
