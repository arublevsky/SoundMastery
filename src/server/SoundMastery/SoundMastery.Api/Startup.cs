using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Validation;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureCors(Configuration);
        services.RegisterDependencies();
        services.AddHttpContextAccessor();
        services.AddIdentity<User, Role>().AddDefaultTokenProviders();
        // ^services.AddIdentity sets default auth scheme to the cookies authentication,
        // so .AddAuthentication must go after this line to override the default to use JWT.
        services.ConfigureAuthentication(Configuration);

        services.ConfigureIdentityOptions();
        services.AddSwaggerGen();
        services.AddDbContext<SoundMasteryContext>();

        services.AddMvc();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
        else
        {
            app.UseHsts();
        }

        app.UseRouting();
        app.UseCors(CorsPolicyName.FrontendApp);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}