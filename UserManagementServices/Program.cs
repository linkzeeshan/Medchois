using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using UserManagementServices.Cores;
using UserManagementServices.Data;
using UserManagementServices.Data.Repository;
using UserManagementServices.Data.Repository.Interfaces;
using UserManagementServices.Dtos;
using UserManagementServices.Models.ViewModel;
using UserManagementServices.Services;
using UserManagementServices.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load the connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//Add config required Email
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});
//Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//configure email setting
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

//Add DI
builder.Services.AddScoped<IUserEmailService, UserEmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserManagementRepo, UserManagementRepo>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
//Jwt configuration ends here

//Add Authorization Button
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "User Management API", Version = "v1" });
option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
});
option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", [Authorize] () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/Register", [AllowAnonymous]async ([FromBody] UserCreateDto model, IUserEmailService _emailService,  IUserManagementRepo _repositoiry, HttpContext context) =>
{
    var response = await _repositoiry.CreateAsync(model);
    var baseURL = context.Request.Host;
    var basepath = context.Request.Path;
    var confirmationLink = $"{baseURL}/{nameof(ConfirmEmail)}?email={model.Email}&token={response.Data.Token}"; //Url.(nameof(ConfirmEmail), "Authentication", new { response.Data.Token, email = model.Email });
    var message = new Message(new string[] { model.Email }, "Confirmation email link", confirmationLink!);
    await _emailService.SendEmailAsyc(message);
    return response;
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/Login", [AllowAnonymous] async ([FromBody] LoginViewModel model, IUserEmailService _emailService, IUserManagementRepo _repositoiry, HttpContext context) =>
{
    var response = await _repositoiry.LoginAsync(model);
    var baseURL = context.Request.Host;
    var basepath = context.Request.Path;

    //var confirmationLink = $"{baseURL}/{nameof(ConfirmEmail)}?email={model.Email}&token={response.Data.Token}";
    var message = new Message(new string[] { model.Email! }, "OTP Confirmation", response.Data.Token);
    await _emailService.SendEmailAsyc(message);
    return response;
})
.WithName("Login")
.WithOpenApi();
app.MapGet("/ConfirmEmail", [AllowAnonymous] async (string email, string token, IUserManagementRepo _repositoiry) =>
{
    var response = await _repositoiry.ConfirnmEmailAsync(email, token);
    return response;
})
.WithName("ConfirmEmail")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

