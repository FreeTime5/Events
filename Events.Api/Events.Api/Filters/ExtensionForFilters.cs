using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Filters;

public static class ExtensionForFilters
{
    public static IServiceCollection AddFilters(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        services.AddScoped<BindingFilter>();

        return services;
    }
}
