using CrescentSchool.API.DataSeed;
using CrescentSchool.API.Entities;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.BLL.Services;
using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace CrescentSchool.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var jwtSettingsSection = builder.Configuration.GetSection("Jwt");

        var loggingConfiguration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(loggingConfiguration)
            .CreateLogger();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettingsSection["Issuer"],
                ValidAudience = jwtSettingsSection["Audience"],
                RoleClaimType = ClaimTypes.Role,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettingsSection["Key"]!)),
            };
        });

        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Host.UseSerilog();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IInsructorService, InstructorService>();
        builder.Services.AddScoped<IInstructorsRepository, InstructorsRepository>();
        builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
        builder.Services.AddScoped<IStudentService, StudentsService>();
        builder.Services.AddScoped<ISessionsRepository, SessionsRepository>();
        builder.Services.AddScoped<ISessionService, SessionService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(
        options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });


        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseCors();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await IdentitySeeder.SeedRolesAndAdminAsync(services);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
