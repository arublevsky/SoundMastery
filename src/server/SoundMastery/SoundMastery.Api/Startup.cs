using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Profile;
using SoundMastery.Application.Validation;
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
            services.AddDatabaseServices(Configuration);

            services.AddIdentity<User, Role>().AddDefaultTokenProviders();

            // .AddIdentity sets default auth scheme to cookies auth, so .AddAuthentication must go after that.
            services.ConfigureAuthentication(Configuration);

            services.ConfigureIdentityOptions();

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
    }
}
