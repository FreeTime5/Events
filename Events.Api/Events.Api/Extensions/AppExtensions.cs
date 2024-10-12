using Events.Api.ApiServices.CookieService;
using Events.Api.ApiServices.CookieService.Implementations;
using Events.Api.ApiServices.EmailService.Implementations;
using Events.Api.ApiServices.EmailService;
using Events.Api.Middlewares;
using Events.Application.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Events.Api.Extensions;

public static class AppExtensions
{
    public static async Task UseDevelopment(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()!;
            var roles = app.Configuration.GetSection("Roles").Get<string[]>()!;

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }

    public static IServiceCollection AddImager(this WebApplicationBuilder builder)
    {
        var imageFolder = Path.Combine(builder.Environment.ContentRootPath, "wwwroot\\EventImages");
        builder.Services.AddImageService(Path.Combine(imageFolder, "default_image.jpg"), imageFolder);
        return builder.Services;
    }

    public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddAppAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["Authorization"];
                    return Task.CompletedTask;
                }
            };
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
            };
        }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        });

        return builder.Services;
    }

    public static IServiceCollection AddAppAthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        });

        return services;
    }

    public static IServiceCollection AddAppCookieService(this IServiceCollection services)
    {
        return services.AddScoped<ICookieService, CookieService>();
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var email = configuration.GetValue<string>("EmailInformation:Email");
        var password = configuration.GetValue<string>("EmailInformation:Password");
        var serverHost = configuration.GetValue<string>("EmailInformation:ServerHost");

        services.AddTransient<IEmailService, EmailService>(provider => new EmailService(email, password, serverHost));
        return services;
    }
}
