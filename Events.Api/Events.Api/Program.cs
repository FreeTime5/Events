using Events.Api.Extensions;
using Events.Api.Filters;
using Events.Application.Extensions;
using Events.Infrastructure.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddFilters()
    .AddAppIdentity()
    .AddAppUseCases()
    .AddAppCookieService()
    .AddAppAthorization()
    .AddInfrastructure(builder.Configuration, builder.Environment);

builder.AddAppAuthentication();

builder.Services.AddAppCors(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();

app.UseAppExceptionHandler();

app.UseCors("ClientApp");

app.ApplyMigrations();

await app.UseDevelopment();

await app.CreateAdmin();

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

