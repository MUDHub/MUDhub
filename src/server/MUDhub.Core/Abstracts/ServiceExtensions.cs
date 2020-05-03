using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MUDhub.Core.Configurations;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUDhub.Core.Abstracts
{
    public static class ServiceExtensions
    {


        public static IServiceCollection AddMudServices(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.AddUserManagment();
            services.AddMudGameManagment();
            return services;
        }

        /// <summary>
        /// Adds all Services they relate to the mudhub UserManagement, e.g. 
        /// LoginService, UserManagent
        /// </summary>
        /// <param name="services"> the service collection in which services are added.</param>
        /// <returns>the given service collection</returns>
        public static IServiceCollection AddUserManagment(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.TryAddSingleton<ILoginService, LoginService>();
            services.TryAddSingleton<IUserManager, UserManager>();
            services.TryAddSingleton<IEmailService, EmailService>();
            return services;
        }

        /// <summary>
        /// Adds all Services they relate to the mudhub MudManagement, e.g. 
        /// GameService, MudManagement
        /// </summary>
        /// <param name="services"> the service collection in which services are added.</param>
        /// <returns>the given service collection</returns>
        public static IServiceCollection AddMudGameManagment(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.TryAddSingleton<IMudManager, MudManager>();
            services.TryAddSingleton<IGameService, GameService>();


            return services;
        }


        public static IServiceCollection AddTargetDatabase(this IServiceCollection services,IConfiguration configuration, DatabaseConfiguration conf)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (conf is null)
                throw new ArgumentNullException(nameof(conf));

            //Todo: Later change this to Scoped!
            var lifetime = ServiceLifetime.Singleton;

            switch (conf.Provider)
            {
                case DatabaseProvider.Sqlite:
                {
                    services.AddDbContext<MudDbContext>(options =>
                        options.UseSqlite(conf.ConnectionString, b =>
                        {
                            b.MigrationsAssembly("MUDhub.Server");
                        }),lifetime);
                    break;
                }
                case DatabaseProvider.MySql:
                case DatabaseProvider.MariaDB:
                {
                    var mysqlConString = configuration.GetValue<string>("MYSQLCONNSTR_localdb");
                    if (!(mysqlConString is null))
                    {
                        conf.ConnectionString = mysqlConString;
                        Console.WriteLine($"MysqlConnectionstring:'{mysqlConString}'");
                    }
                    services.AddDbContext<MudDbContext>(options =>
                        options.UseMySql(conf.ConnectionString, b =>
                        {
                            b.MigrationsAssembly("MUDhub.Server");
                        }), lifetime);
                    break;
                }
                case DatabaseProvider.MsSql:
                {
                    services.AddDbContext<MudDbContext>(options =>
                       options.UseSqlServer(conf.ConnectionString, b =>
                       {
                           b.MigrationsAssembly("MUDhub.Server");
                       }), lifetime);
                    break;
                }
                default:
                    throw new ArgumentException($"No Supported Database Provider is used.", nameof(conf));
            }
            services.AddHostedService<DatabaseInitializer>();
            return services;
        }

        public static IServiceCollection AddServerConfiguration(this IServiceCollection services, IConfiguration conf)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (conf is null)
                throw new ArgumentNullException(nameof(conf));
            services.Configure<ServerConfiguration>(conf.GetSection("Server"));
            services.Configure<DatabaseConfiguration>(conf.GetSection("Server:Database"));
            services.Configure<SpaConfiguration>(conf.GetSection("Server:Spa"));
            services.Configure<MailConfiguration>(conf.GetSection("Server:Mail"));
            return services;
        }
    }
}
