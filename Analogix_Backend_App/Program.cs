using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.ApplicationCore.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Infrastructure.Database;
using Analogix_Backend_App.Infrastructure.Database.Repositories;
using Analogix_Backend_App.Presentation.WebAPI.Configs;
using Analogix_Backend_App.Presentation.WebAPI.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Tools
builder.Services.AddSingleton<TokenTool>();
// Add services.
builder.Services.AddScoped<IUserService, UserService>(); // This line registers the IUserService interface and its implementation UserService with the dependency injection container, allowing for the injection of IUserService into controllers and other services.
builder.Services.AddScoped<IPlayerProfileService, PlayerProfileService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventSubscriptionService, EventSubscriptionService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IEventFaqService, EventFaqService>();
builder.Services.AddScoped<IPlayerReportService, PlayerReportService>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventSubscriptionRepository, EventSubscriptionRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IEventFaqRepository, EventFaqRepository>();
builder.Services.AddScoped<IPlayerReportRepository, PlayerReportRepository>();

// - DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});

builder.Services.AddControllers();


// Configuration of JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    byte[] secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]!);

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // ↓ Values for validating the token, such as the issuer, audience, and signing key, are retrieved from the configuration settings.
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        ValidAudience = builder.Configuration["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                        // ↓ Rules for validating the token, such as ensuring the issuer and audience are correct, the signing key is valid, and the token has not expired.
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                    };
                });

// Admin Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
    policy.RequireRole(nameof(UserRoles.Admin)));
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Analogix",
            Version = "v1",
            Description = "Analogix Platform - Find friends to Play"
        };
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {

        policy.WithOrigins("http://localhost:5175")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication(); // This line adds the authentication middleware to the HTTP request pipeline, enabling the application to authenticate users based on the configured JWT authentication scheme. It ensures that incoming requests are processed for authentication before reaching the authorization middleware.

app.UseAuthorization();

app.MapControllers();

app.Run();
