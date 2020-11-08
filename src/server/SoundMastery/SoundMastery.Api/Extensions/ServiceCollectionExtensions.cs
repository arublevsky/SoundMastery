using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.DataAccess.Services;

namespace SoundMastery.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
                {
                    // need to set both schemes to make JWT work, otherwise cookies scheme is used
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    };
                });
        }

        public static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // sign in
                options.SignIn.RequireConfirmedEmail = true;
            });
        }

        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            DatabaseEngine engine = GetDatabaseEngine(configuration);

            ValidateDatabaseSettings(engine, configuration);

            services.AddSingleton<DatabaseEngineAccessor>(() => engine);
            services.AddTransient<IUserRepository, UserRepository>();
        }

        private static void ValidateDatabaseSettings(DatabaseEngine engine, IConfiguration configuration)
        {
            switch (engine)
            {
                case DatabaseEngine.Postgres:
                    EnsureConnectionStringSpecified("PostgresDatabaseConnection", configuration);
                    break;
                case DatabaseEngine.SqlServer:
                    EnsureConnectionStringSpecified("SqlServerDatabaseConnection", configuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}");
            }
        }

        private static DatabaseEngine GetDatabaseEngine(IConfiguration configuration)
        {
            string engineConfig = configuration["DatabaseSettings:Engine"];
            return Enum.TryParse<DatabaseEngine>(engineConfig, ignoreCase: true, out var result)
                ? result
                : DatabaseEngine.Postgres;
        }

        private static void EnsureConnectionStringSpecified(string name, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Connection string {name} is not configured");
            }
        }
    }
}
