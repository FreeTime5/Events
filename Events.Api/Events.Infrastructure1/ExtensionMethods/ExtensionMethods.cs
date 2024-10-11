using Events.Application.Interfaces;
using Events.Application.Interfaces.RepoInterfaces;
using Events.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.ExtensionMethods;

public static class ExtensionMethods
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepo, EventRepo>();
        services.AddScoped<IMemberRepo, MemberRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        return services;
    }
}
