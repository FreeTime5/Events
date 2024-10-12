using Events.Api.Extensions;
using Events.Api.Filters;
using Events.Application.Extensions;
using Events.Domain.Entities;
using Events.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
    .AddAppServices()
    .AddAppCookieService()
    .AddAppAthorization()
    .AddEmailService(builder.Configuration);

builder.AddAppAuthentication();

builder.Services.AddCors(options => options.AddPolicy("ClientApp", policy =>
{
    policy.WithOrigins(builder.Configuration.GetValue<string>("ClientAppUrl")!)
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseAppExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.UseDevelopment();
}


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Map("/", (HttpResponse response) =>
{
    response.Redirect(app.Configuration.GetValue<string>("ClientAppUrl")!);
});

app.Run();
