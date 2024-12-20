﻿using Events.Api.ApiServices.CookieService;
using Events.Api.ApiServices.CookieService.Implementations;
using Events.Api.Middlewares;
using Events.Application.UseCases.AccountUseCases.AddAdminUseCase;
using Events.Domain.Entities;
using Events.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Events.Api.Extensions;

public static class AppExtensions
{
    public static async Task CreateAdmin(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var addAdminUseCase = scope.ServiceProvider.GetRequiredService<IAddAdminUseCase>();
            await addAdminUseCase.Execute(app.Configuration.GetValue<string>("Admin:Password"));
        }
    }

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

    public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddImager(this WebApplicationBuilder builder)
    {
        var imageFolder = Path.Combine(builder.Environment.ContentRootPath, "wwwroot\\EventImages");

        return builder.Services;
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

    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options => options.AddPolicy("ClientApp", policy =>
        {
            policy.WithOrigins(configuration.GetValue<string>("ClientAppUrl")!)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        return services;
    }

    public static IServiceCollection AddAppCookieService(this IServiceCollection services)
    {
        return services.AddScoped<ICookieService, CookieService>();
    }

    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<Member, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}
