using EventHub.Event.Application.Abstractions;
using EventHub.Event.Application.Exceptions;
using EventHub.Event.Application.Services;
using EventHub.Event.Application.Validators;
using EventHub.Event.Infrastructure.Persistence;
using EventHub.Event.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<EventAppService>();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000", "https://*.app.github.dev")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseExceptionHandler(errApp => errApp.Run(async context =>
{
    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var (status, title) = ex switch
    {
        NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
        _ => (StatusCodes.Status500InternalServerError, "Ein unerwarteter Fehler ist aufgetreten.")
    };
    context.Response.StatusCode = status;
    await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = title });
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.MapControllers();
app.Run();