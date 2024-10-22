using Events.Application.Models.Account;
using Events.Application.Models.Event;
using Events.Application.Models.Member;
using Events.Application.Services.Account;
using Events.Application.Services.AccountService.Implementations;
using Events.Application.Services.AutoMapper;
using Events.Application.Services.CategoryService;
using Events.Application.Services.CategoryService.Implementations;
using Events.Application.Services.EventService;
using Events.Application.Services.EventService.Implementations;
using Events.Application.Services.ImageService;
using Events.Application.Services.ImageService.Implementations;
using Events.Application.Services.MemberService;
using Events.Application.Services.MemberService.Implementations;
using Events.Application.Validators.Account;
using Events.Application.Validators.Event;
using Events.Application.Validators.Member;
using Events.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Events.Application.Extensions;

public static class ExtensionMethods
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddUnitOfWork();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddAutoMapper(typeof(ApplicationProfile));

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateEventRequestDTO>, CreateEventRequestValidator>();
        services.AddScoped<IValidator<UpdateEventRequestDTO>, UpdateEventRequestValidator>();
        services.AddScoped<IValidator<DeleteAndAddMemberRequestDTO>, DeleteAndAddMemberRequestValidator>();
        services.AddScoped<IValidator<LogInRequestDTO>, LogInRequestValidator>();
        services.AddScoped<IValidator<RegisterRequestDTO>, RegisterRequestValidator>();

        return services;
    }

    public static IServiceCollection AddImageService(this IServiceCollection services, string defaultImagePath, string imageFolder)
    {
        services.AddSingleton<IImageService, ImageService>(provider => new ImageService(defaultImagePath, imageFolder));

        return services;
    }
}
