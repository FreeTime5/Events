using Events.Api.Extensions;
using Events.Api.Filters;
using Events.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppDbContext(builder.Configuration);

builder.Services.AddAppIdentity();


builder.AddImager()
    .AddFilters()
    .AddValidators()
    .AddAppServices()
    .AddAppCookieService()
    .AddAppAthorization()
    .AddEmailService(builder.Configuration);

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

