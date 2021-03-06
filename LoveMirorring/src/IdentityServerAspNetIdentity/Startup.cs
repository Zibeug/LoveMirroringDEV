﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using IdentityServer4.Validation;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.Services;
using IdentityServerAspNetIdentity.Services.RolesAndClaims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Twilio;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public static IConfiguration Configuration { get; private set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddDbContext<LoveMirroringContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            // Ajout d'un certificat pour Identity Server 4
            if (Environment.IsDevelopment())
            {
                // SEULEMENT POUR ENV DEVELOPPEMENT
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                X509Certificate2 cert = null;
                using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    certStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certCollection = certStore.Certificates.Find(
                        X509FindType.FindByThumbprint,
                        // Replace below with your cert's thumbprint
                        Configuration["CertificationThumbprint"],
                        false);
                    // Get the first cert with the thumbprint
                    if (certCollection.Count > 0)
                    {
                        cert = certCollection[0];
                        Log.Logger.Information($"Successfully loaded cert from registry: {cert.Thumbprint}");
                    }
                }

                // Fallback to DeveloperSigningCredential
                if (cert == null)
                {
                    Log.Logger.Error($"Certification not found, going to use DeveloperSigningCredential NOT GOOD");
                    builder.AddDeveloperSigningCredential();
                }
                else
                {
                    builder.AddSigningCredential(cert);
                }
            }

            services.AddAuthentication()
                /*
                 * Auteur : Sébastien Berger 
                 * Date : 18.05.2020
                 * Description : Ajouter l'authentification externe via facebook
                 */
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Facebook:AppID"];
                    facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
                    facebookOptions.SaveTokens = true;
                    facebookOptions.Scope.Add("user_birthday");
                    facebookOptions.Scope.Add("user_gender");
                    facebookOptions.Fields.Add("birthday");
                    facebookOptions.Fields.Add("gender");
                });
                //.AddGoogle(options =>
                //{
                //    // register your IdentityServer with Google at https://console.developers.google.com
                //    // enable the Google+ API
                //    // set the redirect URI to http://localhost:5000/signin-google
                //    options.ClientId = "copy client ID from Google here";
                //    options.ClientSecret = "AIzaSyD-YSbwpf8n11acjTZL8RknbBACSLfgIIw";
                //});
                

            //SeedData.EnsureSeedData(Configuration.GetConnectionString("DefaultConnection"));

            // requires
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            var accountSid = Configuration["Twilio:AccountSID"]; 
            var authToken = Configuration["Twilio:AuthToken"]; 
            TwilioClient.Init(accountSid, authToken);
            services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));

            // Sert à récupéter l'adresse IP du user
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            /*
             *      Auteur : Tim Allemann
             *      2020.05.08
             *      Rajoute les claims identity server 4 au claims d'identity
             *      Permet d'utiliser des policy pour gérer les accès des controlleurs
             */
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, AspNetIdentityProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}