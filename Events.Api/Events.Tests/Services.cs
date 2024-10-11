using Events.Api.Extensions;
using Events.Api.Filters;
using Events.Application.Extensions;
using Events.Domain.Entities;
using Events.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Events.Tests
{
    internal class Services
    {
        public IServiceProvider Provider { get; }

        public Services()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Configuration.AddUserSecrets(Assembly.Load("Events.Tests"));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabase")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            builder.AddImager()
                .AddFilters()
                .AddValidators()
                .AddServices()
                .AddAppCookieService()
                .AddAppAthorization();

            builder.AddAppAuthentication();

            builder.Services.AddCors(options => options.AddPolicy("ClientApp", policy =>
            {
                policy.WithOrigins(builder.Configuration.GetValue<string>("ClientAppUrl")!)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            Provider = builder.Services.BuildServiceProvider();
        }
    }
}
