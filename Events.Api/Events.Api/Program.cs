using Events.Application.Servicies.EventService.DTOs;
using Events.Application.Servicies.EventService.Validators;
using Events.Application.Servicies.Profiles;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ApplicationProfile));
builder.Services.AddScoped<IValidator<CreateEventRequestDTO>, CreateEventRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateEventRequestDTO>, UpdateEventRequestValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
