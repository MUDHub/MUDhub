using Microsoft.Extensions.DependencyInjection;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Adds all Services they relate to the mudhub usermanagent, e.g. 
        /// LoginService, UserManagent
        /// </summary>
        /// <param name="services"> the service collection in which services are added.</param>
        /// <returns>the given service collection</returns>
        public static IServiceCollection AddUserManagment(this IServiceCollection services)
        {
            //Todo: Later change this to scoped.
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IUserManager, UserManager>();
            return services;
        }
    }
}
