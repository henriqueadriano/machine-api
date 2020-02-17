using machine_api.Helpers;
using machine_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace machine_api
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
            //Congurations for Token Properties on appsettings.json
            var tokenConfigSection = Configuration.GetSection("TokenConfig");
            services.Configure<TokenConfig>(tokenConfigSection);

            //configure jwt authentication
            var tokenConfig = tokenConfigSection.Get<TokenConfig>();
            var key = Encoding.ASCII.GetBytes(tokenConfig.secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            /*
             # AddControllers() #
             To get around this, I showed in a previous post how you could create a 
             stripped down version of AddMvc(), only adding the things you really need 
             for creating Web APIs. The AddControllers() extension method now does exactly that - 
             it adds the services required to use Web API Controllers, and nothing more. 
             So you get Authorization, Validation, formatters, and CORS for example, 
             but nothing related to Razor Pages or view rendering. 
             For the full details of what's included see the source code on GitHub.
             */
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
