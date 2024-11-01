using Events.DataAccess;
using Events.DataAccess.Extensions;
using Events.Infrastructure.Services.EmailService;
using Events.Infrastructure.Services.EmailService.Implementations;
using Events.Infrastructure.UnitOfWork;
using Events.Infrastructure.UnitOfWork.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Events.Infrastructure.Extensions;

public static class ExtensionMethods
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var email = configuration.GetSection("EmailInformation:Email").Value;
        var password = configuration.GetSection("EmailInformation:Password").Value;
        var serverHost = configuration.GetSection("EmailInformation:ServerHost").Value;

        services.AddSingleton<IEmailService, EmailService>(provider => new EmailService(email, password, serverHost));
        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDbContext(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

        return services;
    }   
}
