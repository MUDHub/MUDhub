using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;

namespace MUDhub.Server
{
    public class Startup
    {
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _serverConfiguration = configuration.GetSection("Server").Get<ServerConfiguration>();
            CheckConfiguration();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("1");
            //Costume Service Extensions, in Server Project
            services.AddTargetDatabase(_serverConfiguration.Database);
            services.AddServerConfiguration(_configuration);

            Console.WriteLine("2");
            //Mud game Services
            services.AddMudServices();

            Console.WriteLine("3");
            //Add AspnetCore Common Services
            services.AddControllers();
            services.AddSpaStaticFiles(conf => conf.RootPath = _serverConfiguration.Spa.RelativePath);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MUDhub API", Version = "v1" }));

            Console.WriteLine("4");
            AddJwtAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var hostSpa = _serverConfiguration.Spa.IntegratedHosting;
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseSpaStaticFiles();

            app.UseFileServer(true);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MUDhub API");
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            if (hostSpa && env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins(_serverConfiguration.Spa.ExternalHostingUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

            if (hostSpa && !env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = _serverConfiguration.Spa.RelativePath;
                });

            }
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_serverConfiguration.TokenSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.

                // Sending the access token in the query string is required due to
                // a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
                // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                // for more information about security considerations when using
                // the query string to transmit the access token.
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private void CheckConfiguration()
        {
            if (_serverConfiguration is null)
            {
                LogMessage("Server");
                TerminateMessage();
                return;
            }
        }

        private static void TerminateMessage()
        {
            throw new InvalidProgramException("Can't startup the Server no valid configuration found.");
        }

        private void LogMessage(string configPath)
        {
            Console.ForegroundColor =ConsoleColor.DarkRed;
            Console.Write("Error in Configuration:");
            Console.ResetColor();
            Console.Write($"'{configPath}' has no valid value!");
            Console.WriteLine();
        }
    }
}
