using Events.Infrastructure.Repositories.CategoryRepository;
using Events.Infrastructure.Repositories.CategoryRepository.Implementations;
using Events.Infrastructure.Repositories.EventRepository;
using Events.Infrastructure.Repositories.EventRepository.Implementations;
using Events.Infrastructure.Repositories.MemberRepository;
using Events.Infrastructure.Repositories.MemberRepository.Implemantations;
using Events.Infrastructure.Repositories.RegistrationRepository;
using Events.Infrastructure.Repositories.RegistrationRepository.Implementations;
using Events.Infrastructure.UnitOfWorkPattern;
using Microsoft.Extensions.DependencyInjection;


namespace Events.Infrastructure.Extensions;

public static class ExtensionMethods
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IMemberRepository, MemberRepository>();
        services.AddTransient<IRegistrationRepository, RegistrationRepository>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWorkPattern.Implementations.UnitOfWork>();
        return services;
    }
}
