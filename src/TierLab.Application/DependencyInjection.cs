using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TierLab.Application.UseCases.Tierlists;

namespace TierLab.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Register all FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        // Use Cases — Tierlists
        services.AddScoped<ITierlistQueries, TierlistQueries>();

        return services;
    }
}
