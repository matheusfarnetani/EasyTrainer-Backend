using API.Auth;
using Application;
using Infrastructure;
using Domain.API.Interfaces;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAdminCredentialProvider, AdminCredentialProvider>();


// Application and Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Health Check
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
