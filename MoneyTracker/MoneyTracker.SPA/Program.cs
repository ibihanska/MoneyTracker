using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using MoneyTracker.Application;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Infrastructure;
using MoneyTracker.Infrastructure.Services;
using MoneyTracker.Persistence;
using MoneyTracker.SPA.Common;
using MoneyTracker.SPA.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionFilter>();
}).AddFluentValidation();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Authentication:Domain"];
    options.Audience = builder.Configuration["Authentication:Audience"];
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "API",
            Version = "v1",
            Description = "A REST API",
            TermsOfService = new Uri("https://lmgtfy.com/?q=i+like+pie")
        });
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl =
                        new Uri(builder.Configuration["Authentication:Domain"] + "authorize?audience=" +
                                builder.Configuration["Authentication:Audience"]),
                    TokenUrl = new Uri(builder.Configuration["Authentication:Domain"] + "oauth/token")
                }
            }
        });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOptions<StorageConnectionOptions>().Bind(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(options => options
    .WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
    c.OAuthClientId(builder.Configuration["Authentication:ClientId"]);
    c.OAuthUsePkce();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
