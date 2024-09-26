using Events.Application.Interfaces;
using Events.Application.Servicies.EventService.DTOs;
using Events.Application.Servicies.EventService.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application.Servicies.ExtensionMethods;

public static class ExtantionMethods
{
    public static IServiceCollection AddApplicationServicies(this IServiceCollection services)
    {
        services.AddScoped<EventService.EventService>();
        services.AddScoped<MemberService.MemberService>();
        services.AddScoped<IAccountService, AccountService.AccountService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateEventRequestDTO>, CreateEventRequestValidator>();
        services.AddScoped<IValidator<UpdateEventRequestDTO>, UpdateEventRequestValidator>();

        return services;
    }
}
