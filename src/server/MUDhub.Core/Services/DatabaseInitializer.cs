using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Abstracts.Models.Areas;
using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Abstracts.Models.Inventories;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class DatabaseInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger? _logger;
        private readonly DatabaseConfiguration _options;

        public DatabaseInitializer(IServiceScopeFactory scopeFactory,
                                    IOptions<DatabaseConfiguration> options,
                                    ILogger<DatabaseInitializer>? logger = null)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<MudDbContext>();
            var mudManager = serviceScope.ServiceProvider.GetRequiredService<IMudManager>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<IUserManager>();
            var areaManager = serviceScope.ServiceProvider.GetRequiredService<IAreaManager>();
            var itemManager = serviceScope.ServiceProvider.GetRequiredService<IItemManager>();

            if (_options.DeleteDatabase)
            {
                _logger?.LogWarning("Database will be deleted.");
                context.Database.EnsureDeleted();
            }
            if (context.Database.IsSqlite())
            {
                _logger?.LogWarning("The Server may has a new Data schema Version, sqlite don't support some Migration operations. " +
                                    "The old database must be deleted and a will be automatically created. " +
                                    $"Delete database manually or set '{nameof(DatabaseConfiguration.DeleteDatabase)}' option to 'true'.");
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }

            var userExists = true;
            if (_options.CreateDefaultUser)
            {
                userExists = await CreateDefaultUserAsnyc(context, userManager)
                         .ConfigureAwait(false);
            }

            if (_options.CreateDefaultMudData && userExists)
            {
                var actualMud = await context.MudGames.FirstOrDefaultAsync(m => m.Name == "Thors World").ConfigureAwait(false);
                if (actualMud is null)
                {
                    await CreateDefaultMudDataAsync(context, mudManager, areaManager)
                        .ConfigureAwait(false);
                }
                else
                {
                    _logger?.LogWarning($"DefaultMud: {actualMud.Name} already exists!");
                }
            }
            if (_options.CreateDefaultDhbwMudData && userExists)
            {
                var actualMud = await context.MudGames.FirstOrDefaultAsync(m => m.Name == "DHBW Horb").ConfigureAwait(false);
                if (actualMud is null)
                {
                    await CreateDefaultDhbwMudDataAsync(context, mudManager, areaManager, itemManager)
                    .ConfigureAwait(false);
                }
                else
                {
                    _logger?.LogWarning($"DefaultDHBWMud: {actualMud.Name} already exists!");
                }
            }
        }

        private async Task CreateDefaultDhbwMudDataAsync(MudDbContext context, IMudManager mudManager, IAreaManager areaManager, IItemManager itemManager)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                                        .ConfigureAwait(false);
            var resultGame1 = await mudManager.CreateMudAsync("DHBW Horb", new MudCreationArgs
            {
                AutoRestart = true,
                Description = "Duale Hochschule Stuttgart Campus Horb!",
                ImageKey = "some awesome key",
                IsPublic = true,
                OwnerId = user.Id
            }).ConfigureAwait(false);

            var resultArea1 = await areaManager.CreateAreaAsync(user.Id, resultGame1!.Id, new AreaArgs()
            {
                Name = "Erdgeschoss",
                Description = "Eintrittsebene mit Kantine, Außenbereich und Vorlesungsräumen."
            }).ConfigureAwait(false);
            var resultArea2 = await areaManager.CreateAreaAsync(user.Id, resultGame1!.Id, new AreaArgs()
            {
                Name = "1. Stock",
                Description = "Das Stockweg der Elektrotechniker."
            }).ConfigureAwait(false);

            var resultRoom1 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area!.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Ende des Flurs. Hier gehts zum Raucherbereich.",
                IsDefaultRoom = false,
                X = 2,
                Y = 0
            }).ConfigureAwait(false);
            var resultRoom2 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Außenbereich",
                Description = "Platz zum Sonnen. Ausgestattet mit einem Tischkicker und vielen Sonneliegen.",
                IsDefaultRoom = false,
                X = 3,
                Y = 0
            }).ConfigureAwait(false);
            var resultRoom3 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Vorlesungsraum 0/1",
                Description = "Vorlesungsraum der Maschinenbauer. Man kann die Werkstatt durch das Fenster sehen.",
                IsDefaultRoom = false,
                X = 0,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom4 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Hausmeister-Raum",
                Description = "Egal ob loser Bilderrahmen oder tropfendes Waschbecken. Der Hausmeister hat immer eine Lösung.",
                IsDefaultRoom = false,
                X = 1,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom5 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Hier steh ein Getränke- und Süßigkeiten Automat. Yummy...",
                IsDefaultRoom = false,
                X = 2,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom6 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Sitzbereich",
                Description = "Falls es draußen regnet kann man sich auch hier entspannen.",
                IsDefaultRoom = false,
                X = 3,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom7 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Toiletten",
                Description = "Einzige Toiletten des Stockwerkes. Für Mann, Frau und Divers.",
                IsDefaultRoom = false,
                X = 4,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom8 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Vorlesungsraum 5/1",
                Description = "Vorlesungsraum der Maschinenbauer. Man kann die Feuerwehr durch das Fenster sehen.",
                IsDefaultRoom = false,
                X = 5,
                Y = 1
            }).ConfigureAwait(false);
            var resultRoom9 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Ende des Flurs. Man kann auf den Parkplatz und in die Werkstatt gehen.",
                IsDefaultRoom = false,
                X = 0,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom10 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Achtung: Sehr windig wegen dem Eingang.",
                IsDefaultRoom = false,
                X = 1,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom11 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Zentrums des Flures. Man kann in alle Richtungen gehen.",
                IsDefaultRoom = false,
                X = 2,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom12 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Langweiliger Flur-Abteil",
                IsDefaultRoom = false,
                X = 3,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom13 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Yummy: Der Geruch der Kantine erfüllt den ganzen Flur.",
                IsDefaultRoom = false,
                X = 4,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom14 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Flur",
                Description = "Ende des Flurs. Man kann zu der Feuerwehr gehen.",
                IsDefaultRoom = false,
                X = 5,
                Y = 2
            }).ConfigureAwait(false);
            var resultRoom15 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Eingang",
                Description = "Es gibt in der DHBW Horb nur diesen Eingang.",
                IsDefaultRoom = true,
                X = 1,
                Y = 3
            }).ConfigureAwait(false);
            var resultRoom16 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Treppenhaus",
                Description = "Von hier gelangt man in alle anderen Stockwerke.",
                IsDefaultRoom = false,
                X = 2,
                Y = 3
            }).ConfigureAwait(false);
            var resultRoom17 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Kantine",
                Description = "Frei nach dem Motto: Ohne Mampf kein Kampf.",
                IsDefaultRoom = false,
                X = 4,
                Y = 3
            }).ConfigureAwait(false);
            var resultRoom18 = await areaManager.CreateRoomAsync(user.Id, resultArea1.Area.Id, new RoomArgs()
            {
                Name = "Vorlesungsraum 5/3",
                Description = "Vorlesungsraum der Maschinenbauer. Man kann die Feuerwehr und die Straße durch das Fenster sehen.",
                IsDefaultRoom = false,
                X = 5,
                Y = 3
            }).ConfigureAwait(false);

            var resultConnection1 = await areaManager.CreateConnectionAsync(user.Id, resultRoom1.Room!.Id, resultRoom2.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Außenbereich",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection2 = await areaManager.CreateConnectionAsync(user.Id, resultRoom1.Room!.Id, resultRoom5.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection3 = await areaManager.CreateConnectionAsync(user.Id, resultRoom2.Room!.Id, resultRoom6.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Sitzbereich <=> Außenbereich",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection4 = await areaManager.CreateConnectionAsync(user.Id, resultRoom3.Room!.Id, resultRoom9.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Vorlesungsraum 0/1",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection5 = await areaManager.CreateConnectionAsync(user.Id, resultRoom4.Room!.Id, resultRoom10.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Hausmeister-Raum",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection6 = await areaManager.CreateConnectionAsync(user.Id, resultRoom4.Room!.Id, resultRoom5.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Hausmeister-Raum",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection7 = await areaManager.CreateConnectionAsync(user.Id, resultRoom5.Room!.Id, resultRoom6.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Sitzbereich",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection8 = await areaManager.CreateConnectionAsync(user.Id, resultRoom5.Room!.Id, resultRoom11.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection9 = await areaManager.CreateConnectionAsync(user.Id, resultRoom6.Room!.Id, resultRoom12.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Sitzbereich",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection10 = await areaManager.CreateConnectionAsync(user.Id, resultRoom7.Room!.Id, resultRoom13.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Toiletten",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection11 = await areaManager.CreateConnectionAsync(user.Id, resultRoom8.Room!.Id, resultRoom14.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Vorlesungsraum 5/1",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection12 = await areaManager.CreateConnectionAsync(user.Id, resultRoom9.Room!.Id, resultRoom10.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection13 = await areaManager.CreateConnectionAsync(user.Id, resultRoom10.Room!.Id, resultRoom11.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection14 = await areaManager.CreateConnectionAsync(user.Id, resultRoom11.Room!.Id, resultRoom12.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection15 = await areaManager.CreateConnectionAsync(user.Id, resultRoom12.Room!.Id, resultRoom13.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection16 = await areaManager.CreateConnectionAsync(user.Id, resultRoom13.Room!.Id, resultRoom14.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Flur",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection17 = await areaManager.CreateConnectionAsync(user.Id, resultRoom15.Room!.Id, resultRoom10.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Eingang",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection18 = await areaManager.CreateConnectionAsync(user.Id, resultRoom11.Room!.Id, resultRoom16.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Treppenhaus",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection19 = await areaManager.CreateConnectionAsync(user.Id, resultRoom13.Room!.Id, resultRoom17.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Kantine",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
            var resultConnection20 = await areaManager.CreateConnectionAsync(user.Id, resultRoom14.Room!.Id, resultRoom18.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "Flur <=> Vorlesungsraum 5/3",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);

            var item1 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Apfel",
                Description = "An Apple a keeps the doctor away.",
                Weight = 5
            }).ConfigureAwait(false);
            var item2 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Mathe-Buch",
                Description = "Statitik 2",
                Weight = 10
            }).ConfigureAwait(false);
            var item3 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "White-Board",
                Description = "Des Dozenten größter Feind.",
                Weight = 100
            }).ConfigureAwait(false);
            var item4 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Kreide",
                Description = "Weiß und farbig.",
                Weight = 2
            }).ConfigureAwait(false);
            var item5 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Wasserflasche",
                Description = "Stay healthy",
                Weight = 5
            }).ConfigureAwait(false);
            var item6 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Exmatrikulation-Antrag",
                Description = "Der Feind eines jeden Studenten.",
                Weight = 20
            }).ConfigureAwait(false);
            var item7 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Inmatrikulation-Antrag",
                Description = "Der Freund eines jeden Studenten.",
                Weight = 20
            }).ConfigureAwait(false);
            var item8 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Mittagessen",
                Description = "Vegetarisch",
                Weight = 30
            }).ConfigureAwait(false);
            var item9 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "Overhead-Projektor",
                Description = "Relikt aus der Steinzeit.",
                Weight = 15
            }).ConfigureAwait(false);
            var item10 = await itemManager.CreateItemAsync(user.Id, resultGame1!.Id, new ItemArgs()
            {
                Name = "RedBull",
                Description = "Belebt Körper und Geist.",
                Weight = 4
            }).ConfigureAwait(false);
        }

        private async Task CreateDefaultMudDataAsync(MudDbContext context, IMudManager mudManager, IAreaManager areaManager)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                                        .ConfigureAwait(false);
            var resultGame = await mudManager.CreateMudAsync("Thors World", new MudCreationArgs
            {
                AutoRestart = true,
                Description = "It's thors 9 worlds!",
                ImageKey = "some awesome key",
                IsPublic = true,
                OwnerId = user.Id
            }).ConfigureAwait(false);

            var resultArea = await areaManager.CreateAreaAsync(user.Id, resultGame!.Id, new AreaArgs()
            {
                Name = "First Etage",
                Description = "Nice View"
            }).ConfigureAwait(false);

            var resultRoom1 = await areaManager.CreateRoomAsync(user.Id, resultArea.Area!.Id, new RoomArgs()
            {
                Name = "Dinner Room",
                Description = "Yummy Food",
                IsDefaultRoom = true,
                X = 1,
                Y = 1
            }).ConfigureAwait(false);

            var resultRoom2 = await areaManager.CreateRoomAsync(user.Id, resultArea.Area.Id, new RoomArgs()
            {
                Name = "Sleeping Room",
                Description = "Naughty Things to see",
                IsDefaultRoom = false,
                X = 2,
                Y = 1
            }).ConfigureAwait(false);

            var resultConnection = await areaManager.CreateConnectionAsync(user.Id, resultRoom1.Room!.Id, resultRoom2.Room!.Id, new RoomConnectionsArgs()
            {
                Description = "From Dinner to Sleep",
                LockArgs = new LockArgs()
                {
                    LockType = LockType.NoLock
                }
            }).ConfigureAwait(false);
        }

        private async Task<bool> CreateDefaultUserAsnyc(MudDbContext context, IUserManager userManager)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == _options.DefaultMudAdminEmail)
                .ConfigureAwait(false);
            if (user != null)
            {
                _logger?.LogWarning($"DefaultUser {_options.DefaultMudAdminEmail} already exists!");
                return true;
            }

            var registerResult = await userManager.RegisterUserAsync(new RegistrationUserArgs
            {
                Email = _options.DefaultMudAdminEmail,
                Password = _options.DefaultMudAdminPassword,
                Lastname = "",
                Firstname = "DefaultUser"
            }).ConfigureAwait(false);

            if (!registerResult.Success)
            {
                _logger?.LogError(registerResult.Errormessage);
                return false;
            }

            var success = await userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Admin)
                .ConfigureAwait(false);
            if (success)
            {
                _logger?.LogInformation("The role Admin was added to the user");
            }
            success = await userManager.AddRoleToUserAsync(registerResult!.User!.Id, Roles.Master)
                .ConfigureAwait(false);
            if (success)
            {
                _logger?.LogInformation("The role Master was added to the user");
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
