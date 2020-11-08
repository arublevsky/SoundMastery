using System;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.Application.Profile;
using SoundMastery.Application.Validation;
using SoundMastery.DataAccess.Services;
using SoundMastery.DataAccess.Services.Postgres;
using SoundMastery.DataAccess.Services.SqlServer;
using SoundMastery.DataAccess.Stores;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        CorsPolicyName.FrontendApp,
                        builder => builder.WithOrigins("http://localhost:9000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });

            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IUserEmailStore<User>, UserStore>();
            services.AddTransient<IRoleStore<Role>, RoleStore>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            RegisterDatabaseSpecificDependencies(services);

            services.AddIdentity<User, Role>().AddDefaultTokenProviders();

            // .AddIdentity sets default auth scheme to cookies auth, so .AddAuthentication must go after that.
            services.AddAuthentication(x =>
                {
                    // need to set both schemes to make JWT work
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
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    };
                });

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

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<UserProfileValidator>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(CorsPolicyName.FrontendApp);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void RegisterDatabaseSpecificDependencies(IServiceCollection services)
        {
            DatabaseEngine engine = GetDatabaseEngine();

            switch (engine)
            {
                case DatabaseEngine.Postgres:
                    EnsureConnectionStringSpecified("PostgresDatabaseConnection");
                    services.AddTransient<IUserRepository, PgsqlUserRepository>();
                    break;
                case DatabaseEngine.SqlServer:
                    EnsureConnectionStringSpecified("SqlServerDatabaseConnection");
                    services.AddTransient<IUserRepository, SqlServerUserRepository>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}");
            }
        }

        private DatabaseEngine GetDatabaseEngine()
        {
            string engineConfig = Configuration["DatabaseSettings:Engine"];
            return Enum.TryParse<DatabaseEngine>(engineConfig, ignoreCase: true, out var result)
                ? result
                : DatabaseEngine.Postgres;
        }

        private void EnsureConnectionStringSpecified(string name)
        {
            string connectionString = Configuration.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Connection string {name} is not configured");
            }
        }
    }
}
