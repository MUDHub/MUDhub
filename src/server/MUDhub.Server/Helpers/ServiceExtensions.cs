using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUDhub.Core.Services;
using MUDhub.Server.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Helpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTargetDatabase(this IServiceCollection services, DatabaseConfiguration conf)
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
                        options.UseSqlite(conf.ConnectionString), lifetime);
                    break;
                }
                case DatabaseProvider.MySql:
                case DatabaseProvider.MariaDB:
                {
                    services.AddDbContext<MudDbContext>(options =>
                        options.UseMySql(conf.ConnectionString), lifetime);
                    break;
                }
                case DatabaseProvider.MsSql:
                {
                    services.AddDbContext<MudDbContext>(options =>
                       options.UseSqlServer(conf.ConnectionString), lifetime);
                    break;
                }
                default:
                    throw new ArgumentException($"No Supported Database Provider is used.", nameof(conf));
            }
            return services;
        }

        public static IServiceCollection AddServerConfiguration(this IServiceCollection services, ServerConfiguration conf)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (conf is null)
                throw new ArgumentNullException(nameof(conf));
            services.ConfigureOptions(conf);
            services.ConfigureOptions(conf.Database);
            services.ConfigureOptions(conf.Spa);
            return services;
        }
    }
}
