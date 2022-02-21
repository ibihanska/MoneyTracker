using System.Security.Claims;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyTracker.Api.Common;
using MoneyTracker.Api.Services;
using MoneyTracker.Application;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//});
builder.Services.AddControllers();

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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Authentication:Domain"] + "authorize?audience=" + builder.Configuration["Authentication:Audience"]),
                TokenUrl = new Uri(builder.Configuration["Authentication:Domain"] + "oauth/token")
                            
            }
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IDateTime, MachineDateTime>();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(options=>
options.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.OAuthClientId(builder.Configuration["Authentication:ClientId"]);
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
