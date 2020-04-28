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
    }
}
