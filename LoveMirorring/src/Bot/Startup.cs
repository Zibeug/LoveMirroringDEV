// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Builder.EchoBot;
using Microsoft.BotBuilderSamples.Bots;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Microsoft.BotBuilderSamples
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, TextBot>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["URLIdentityServer4"];

                    options.RequireHttpsMetadata = false;

                    options.Audience = "bot1";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<UserState>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrateur", policy => policy.RequireClaim(ClaimTypes.Role, "Administrateur"));
                options.AddPolicy("Utilisateur", policy => policy.RequireClaim(ClaimTypes.Role, "Utilisateur"));
                options.AddPolicy("Moderateur", policy => policy.RequireClaim(ClaimTypes.Role, "Moderateur"));
                options.AddPolicy("Administrateur,Moderateur", policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == "Administrateur" || c.Value == "Moderateur")));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseNamedPipes(System.Environment.GetEnvironmentVariable("https://lovemirroring-bot.azurewebsites.net/") + ".directline");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}