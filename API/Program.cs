using API.Auth;
using Application;
using Infrastructure;
using Domain.API.Interfaces;
using API.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.BackgroundServices;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5151); // HTTP
    serverOptions.ListenAnyIP(7173, listenOptions => listenOptions.UseHttps()); // HTTPS
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add controllers
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAdminCredentialProvider, AdminCredentialProvider>();

// Auth configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
            )
        };
    });


// Application and Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Video Processing Worker
builder.Services.AddHttpClient();
builder.Services.AddHostedService<VideoProcessingWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.Use(async (context, next) =>
{
    var ext = context.Request.Headers["X-External-Request"];
    Console.WriteLine($"X-External-Request: {ext}");
    var isExternal = context.Request.Headers["X-External-Request"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

    if (isExternal)
    {
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "-1"),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "system"),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, "external@system.local")
        };

        var identity = new System.Security.Claims.ClaimsIdentity(claims, "External");
        context.User = new System.Security.Claims.ClaimsPrincipal(identity);
    }

    await next();
});

// Middlewares

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
