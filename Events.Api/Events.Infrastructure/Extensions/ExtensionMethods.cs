using Events.Application.Services.ClaimsService;
using Events.Application.Services.EmailService;
using Events.Application.Services.ImageService;
using Events.Application.Services.TokenService;
using Events.Domain.RepositoryInterfaces;
using Events.Domain.UnitOfWorkInterface;
using Events.Infrastructure.Repositories;
using Events.Infrastructure.Services.ClaimsService;
using Events.Infrastructure.Services.EmailService;
using Events.Infrastructure.Services.ImageService;
using Events.Infrastructure.Services.TokenService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Events.Infrastructure.Extensions;

public static class ExtensionMethods
{
    private static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("EventDatabase")));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var email = configuration.GetSection("EmailInformation:Email").Value;
        var password = configuration.GetSection("EmailInformation:Password").Value;
        var serverHost = configuration.GetSection("EmailInformation:ServerHost").Value;
        services.AddSingleton<IEmailService, EmailService>(provider => new EmailService(email, password, serverHost));

        var imageFolder = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot\\EventImages");
        services.AddSingleton<IImageService, ImageService>(provider => new ImageService(Path.Combine(imageFolder, "default_image.jpg"), imageFolder));

        services.AddSingleton<IClaimsService, ClaimsService>();

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEventRepsository, EventRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();

        services.AddAppDbContext(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddAppDbContext(configuration);
        services.AddServices(configuration, webHostEnvironment);
        services.AddUnitOfWork(configuration);

        return services;
    }
}
