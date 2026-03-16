using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.ApplicationCore.Services;
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

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
