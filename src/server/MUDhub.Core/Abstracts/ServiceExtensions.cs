using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts
{
    public static class ServiceExtensions
    {

        /// <summary>
        public static IServiceCollection AddMudGame(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.AddUserManagment();
            services.AddMudGameCore();
            return services;
        }


        /// Adds all Services they relate to the mudhub usermanagent, e.g. 
        /// LoginService, UserManagent
        /// </summary>
        /// <param name="services"> the service collection in which services are added.</param>
        /// <returns>the given service collection</returns>
        public static IServiceCollection AddUserManagment(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.TryAddSingleton<ILoginService, LoginService>();
            services.TryAddSingleton<IUserManager, UserManager>();
            return services;
        }


        public static IServiceCollection AddMudGameCore(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.TryAddSingleton<IMudManager, MudManager>();
            services.TryAddSingleton<IGameService, GameService>();
            return services;
        }
    }
}
