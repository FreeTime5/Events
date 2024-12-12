using Events.Api.Authorization.Handlers;
using Events.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Events.Api.Authorization.Extension
{
    public static class AuthorazationExtension
    {
        public static IServiceCollection AddAppAthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                options.AddPolicy("RolePolicy", policy =>
                {
                    policy.AddRequirements(new RoleRequirement(["Admin", "User"]));
                });
            });

            services.AddSingleton<IAuthorizationHandler, RolesHandler>();

            return services;
        }
    }
}
