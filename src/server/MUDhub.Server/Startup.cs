using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Costume Service Extensions, in Server Project
            services.AddTargetDatabase(_serverConfiguration.Database);
            services.AddServerConfiguration(_configuration);

            //Mud game Services
            services.AddMudServices();

            //Add AspnetCore Common Services
            services.AddControllers();
            services.AddSpaStaticFiles(conf => conf.RootPath = _serverConfiguration.Spa.RelativePath);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MUDhub API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var hostSpa = _serverConfiguration.Spa.IntegratedHosting;
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseSpaStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MUDhub API");
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            if (!hostSpa && env.IsDevelopment())
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
    }
}
