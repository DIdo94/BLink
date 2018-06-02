using BLink.Core.Constants;
using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Data;
using BLink.Data.Repositories;
using BLink.Models;
using BLink.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLink.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            AppConstants.ConnectionStrings.Add(AppConstants.BlinkConnectionKey, 
                Configuration.GetConnectionString(AppConstants.BlinkConnectionKey)); // Move to better place
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("JWTSettings"));

            services.AddEntityFramework()
                    .AddDbContext<BlinkDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<BlinkDbContext>();

            services.AddDbContext<BlinkDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // avoid redirecting REST clients on 401
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddTransient<IClubsRepository, ClubsRepository>();
            services.AddTransient<IMembersRepository, MembersRepository>();
            services.AddTransient<ICountriesRepository, CountriesRepository>();
            services.AddTransient<IInvitationsRepository, InvitationsRepository>();
            services.AddTransient<IClubEventsRepository, ClubEventsRepository>();

            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<IMembersService, MembersService>();
            services.AddTransient<IClubsService, ClubsService>();
            services.AddTransient<IClubEventsService, ClubEventsService>();
            services.AddTransient<IFileService, FileService>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler();
            app.UseIdentity();

            // secretKey contains a secret passphrase only your server knows
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience
            };
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseMvc();
        }
    }
}
