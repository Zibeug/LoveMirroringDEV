using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mvc.Hubs;
using mvc.Models;
using mvc.Services.RolesAndClaims;
using mvc.ViewModels;

namespace mvc
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public object GlobalHost { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //Cross-origin policy to accept request from localhost:5002.
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    x => x.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            // Besoin de l'utilisation des cookies pour g�rer les authentification avec le protocole OpenIdConnect
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Configuration["URLIdentityServer4"];
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                });

            /*
             *      Auteur : Hans Morsch
             *      11.05.2020
             *      Rajoute les claims identity server 4 au claims d'identity
             *      Permet d'utiliser des policy pour g�rer les acc�s des controlleurs
             */
            services.AddSingleton<Microsoft.AspNetCore.Authentication.IClaimsTransformation, KarekeClaimsTransformer>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrateur", policy => policy.RequireClaim("Role", "Administrateur"));
                options.AddPolicy("Utilisateur", policy => policy.RequireClaim("Role", "Utilisateur"));
                options.AddPolicy("Moderateur", policy => policy.RequireClaim("Role", "Moderateur"));
                options.AddPolicy("Administrateur,Moderateur", policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == "Administrateur" || c.Value == "Moderateur")));
            });

            services.AddSignalR();
            services.AddSingleton<IBotFrameworkHttpAdapter>(new BotFrameworkHttpAdapter());
            // Pour le chat vocal
            services.AddSingleton<List<User>>();
            services.AddSingleton<List<UserCall>>();
            services.AddSingleton<List<CallOffer>>();


            // Pour le chat priv�
            services.AddSingleton<List<ConnectionPC>>();


            //// Create the Bot Framework Adapter.
            //services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();

            //// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            //services.AddTransient<IBot, EchoBot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFileServer();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            //app.UseBotFramework();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<LetsChatHub>("/letschathub");
                endpoints.MapHub<ConnectionHub>("/ConnectionHub", options =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                });
            });

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden)
                    response.Redirect("/Account/AccessDenied");
            });
        }


    }
}
