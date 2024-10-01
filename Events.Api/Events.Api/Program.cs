using Events.Application.Interfaces;
using Events.Application.Servicies.ExtensionMethods;
using Events.Application.Servicies.ImageService;
using Events.Application.Servicies.Profiles;
using Events.Domain.Entities;
using Events.Infrastructure.Data;
using Events.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabase")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


var imageFolder = Path.Combine(builder.Environment.ContentRootPath, "wwwroot\\EventImages");
builder.Services.AddSingleton<IImageService, ImageService>(provider => new ImageService(Path.Combine(imageFolder, "default_image.jpg"), imageFolder));
builder.Services.AddAuthentication();
builder.Services.AddAutoMapper(typeof(ApplicationProfile));
builder.Services.AddValidators();
builder.Services.AddApplicationServicies();
builder.Services.AddRepositories();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
