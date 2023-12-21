using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Authorization.ExternalProviders;
using SoundMastery.Application.Authorization.ExternalProviders.Facebook;
using SoundMastery.Application.Authorization.ExternalProviders.Google;
using SoundMastery.Application.Authorization.ExternalProviders.Microsoft;
using SoundMastery.Application.Authorization.ExternalProviders.Twitter;
using SoundMastery.Application.Common;
using SoundMastery.Application.Common.Files;
using SoundMastery.Application.Core;
using SoundMastery.Application.Identity;
using SoundMastery.Application.Profile;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.DataAccess.Services.Users;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using Tweetinvi.Auth;

namespace SoundMastery.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        var env = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();

        RegisterFileProvider(services, env);

        services.AddTransient<IUserStore<User>, UserStore>();
        services.AddTransient<IUserEmailStore<User>, UserStore>();
        services.AddTransient<IRoleStore<Role>, RolesRepository>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ISystemConfigurationService, SystemConfigurationService>();
        services.AddTransient<IGenericRepository<Material>, GenericRepository<Material>>();
        services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();
        services.AddTransient<IGenericRepository<IndividualLesson>, GenericRepository<IndividualLesson>>();
        services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
        services.AddTransient<IIdentityManager, IdentityManager>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IExternalAuthProviderResolver, ExternalAuthProviderResolver>();
        services.AddTransient<IFacebookService, FacebookService>();
        services.AddTransient<IGoogleService, GoogleService>();
        services.AddTransient<IMicrosoftService, MicrosoftService>();
        services.AddTransient<ITwitterService, TwitterService>();
        services.AddTransient<ICoreService, CoreService>();

        // Singletons
        services.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();
    }

    private static void RegisterFileProvider(IServiceCollection services, IWebHostEnvironment env)
    {
        if (env.IsProduction())
        {
            services.AddTransient<IFileProvider, AzureFileProvider>();
        }
        else
        {
            services.AddTransient<IFileProvider, LocalFileProvider>();
        }
    }

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

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(CorsPolicyName.FrontendApp,
                    builder =>
                    {
                        // while running inside k8s the API is not exposed to outer world
                        // client requests are coming from cluster's localhost
                        builder
                            .WithOrigins(configuration.GetValue<string>("ClientUrl"))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
    }
}