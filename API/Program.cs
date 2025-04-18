using API.Auth;
using Application;
using Infrastructure;
using Domain.Application.Interfaces;
using Domain.Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<IConnectionManager, ConnectionManager>();
builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    var connectionManager = provider.GetRequiredService<IConnectionManager>();
    var connStr = connectionManager.GetCurrentConnectionString();

    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
});



builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureRepositories();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
