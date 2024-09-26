using Events.Application.Interfaces;
using Events.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.ExtensionMethods;

public static class ExtensionMethods
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepo, EventRepo>();
        services.AddScoped<IMemberRepo, MemberRepo>();
        return services;
    }
}
