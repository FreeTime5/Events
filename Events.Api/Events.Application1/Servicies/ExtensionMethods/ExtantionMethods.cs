using Events.Application.Interfaces;
using Events.Application.Services.EventService.DTOs;
using Events.Application.Services.EventService.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application.Services.ExtensionMethods;

public static class ExtantionMethods
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService.EventService>();
        services.AddScoped<IMemberService, MemberService.MemberService>();
        services.AddScoped<IAccountService, AccountService.AccountService>();
        services.AddScoped<ICategoryService, CategoryService.CategoryService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateEventRequestDTO>, CreateEventRequestValidator>();
        services.AddScoped<IValidator<UpdateEventRequestDTO>, UpdateEventRequestValidator>();

        return services;
    }
}
