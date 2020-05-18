using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using MUDhub.Core.Services;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class DatabaseInitializerTest
    {
        [Fact]
        public async Task CreateDataBase()
        {

            var services = new ServiceCollection();
            services.AddDbContext<MudDbContext>(options =>
                            options.UseInMemoryDatabase("Database-DataBaseInitializer"));
            services.AddUserManagment();
            services.AddAreaManagment();
            services.AddMudGameManagment();
            services.AddItemsManagement();
            services.AddCharacterManagment();
            services.AddSingleton<DatabaseInitializer>();

            services.AddOptions();
            services.Configure<ServerConfiguration>(c =>
            {

            });
            services.Configure<DatabaseConfiguration>(c =>
            {
                c.CreateDefaultDhbwMudData = true;
                c.DeleteDatabase = true;
                c.CreateDefaultAdminUser = true;
                c.CreateDefaultMasterUser = true;
                c.DefaultMudAdminEmail = "test@test.de";
                c.DefaultMudAdminPassword = "test";
            });
            services.Configure<SpaConfiguration>(c =>
            {

            });
            services.Configure<MailConfiguration>(c =>
            {

            });

            var provider = services.BuildServiceProvider();
            var databaseInitializer = provider.GetRequiredService<DatabaseInitializer>();
            await databaseInitializer.StartAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
