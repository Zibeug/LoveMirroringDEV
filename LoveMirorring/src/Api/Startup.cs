using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Api.Services.RolesAndClaims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get;}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["URLIdentityServer4"];

                    options.RequireHttpsMetadata = false;

                    options.Audience = "api1";
                });
            services.AddDbContext<LoveMirroringContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            // Sert � r�cup�ter l'adresse IP du user
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //services.AddIdentity<AspNetUser, AspNetRole>(options => options.Stores.MaxLengthForKeys = 128)
            //.AddRoles<AspNetRole>()
            //.AddEntityFrameworkStores<LoveMirroringContext>()
            //.AddDefaultUI()
            //.AddDefaultTokenProviders();

            ////Authorization
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            //});

            /*
             *      Auteur : Tim Allemann
             *      2020.05.08
             *      Rajoute les claims identity server 4 au claims d'identity
             *      Permet d'utiliser des policy pour g�rer les acc�s des controlleurs
             */
            services.AddSingleton<Microsoft.AspNetCore.Authentication.IClaimsTransformation, KarekeClaimsTransformer>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrateur", policy => policy.RequireClaim(ClaimTypes.Role, "Administrateur"));
                options.AddPolicy("Utilisateur", policy => policy.RequireClaim(ClaimTypes.Role, "Utilisateur"));
                options.AddPolicy("Moderateur", policy => policy.RequireClaim(ClaimTypes.Role, "Moderateur"));
            });

            /*
             *      Auteur : Tim Allemann
             *      2020.05.18
             *      Rajoute un service qui tourne touts les x secondes
             */
            services.AddHostedService<NewMatchHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Could Not Find Anything");
            });
        }
    }
}
