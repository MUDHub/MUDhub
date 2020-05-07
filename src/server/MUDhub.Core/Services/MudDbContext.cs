using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Muds;
using System;
using System.Threading.Tasks;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Services
{
    public class MudDbContext : DbContext
    {
        public MudDbContext(DbContextOptions options,
                            IOptions<DatabaseConfiguration> conf = null,
                            ILogger<MudDbContext>? logger = null,
                            bool useNotInUnitests = true)
            : base(options)
        {

            if (useNotInUnitests)
            {
                if (conf?.Value?.DeleteDatabase ?? false)
                {
                    logger?.LogWarning("Database will be deleted.");
                    Database.EnsureDeleted();
                }
                if (Database.IsSqlite())
                {

                    logger?.LogWarning("The Server may has a new Data schema Version, sqlite don't support some Migration operations. " +
                                        "The old database must be deleted and a will be automatically created. " +
                                        $"Delete database manually or set '{nameof(DatabaseConfiguration.DeleteDatabase)}' option to 'true'.");
                    Database.EnsureCreated();
                }
                else
                {
                    Database.Migrate();
                }

            }
            else
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        public DbSet<User> Users { get; set; } = null!;


        public DbSet<MudGame> MudGames { get; set; } = null!;
        public DbSet<MudJoinRequest> MudJoinRequests { get; set; } = null!;

        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<CharacterClass> Classes { get; set; } = null!;
        public DbSet<CharacterRace> Races { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<RoomConnection> RoomConnections { get; set; } = null!;
        public DbSet<RoomInteraction> RoomInteractions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            //Configures MudGame
            modelBuilder.Entity<MudGame>()
                .HasKey(mg => mg.Id);
            modelBuilder.Entity<MudGame>()
                .HasOne(mg => mg.DefaultRoom);
            modelBuilder.Entity<MudGame>()
                .HasMany(mg => mg.Characters)
                .WithOne(c => c.Game);

            //Configures Character
            modelBuilder.Entity<Character>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Race)
                .WithMany(r => r.Characters);
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Class)
                .WithMany(cl => cl.Characters);

            //Configures CharacterClass
            modelBuilder.Entity<CharacterClass>()
                .HasKey(cl => cl.Id);

            //Configures CharacterRace
            modelBuilder.Entity<CharacterRace>()
                .HasKey(r => r.Id);

            //Configures CharacterBoost
            modelBuilder.Entity<CharacterBoost>()
                .HasKey(b => b.Id);

            //Configures MudJoinRequests
            modelBuilder.Entity<MudJoinRequest>()
                .HasKey(mjr => new { mjr.MudId, mjr.UserId });
            modelBuilder.Entity<MudJoinRequest>()
                .HasOne(mjr => mjr.MudGame)
                .WithMany(mg => mg.JoinRequests)
                .HasForeignKey(mjr => mjr.MudId);

            //Configures User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            //Configures Room
            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Area)
                .WithMany(a => a.Rooms);
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Interactions)
                .WithOne(ri => ri.Room);
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Connections);

            //Configures RoomInteraction
            modelBuilder.Entity<RoomInteraction>()
                .HasKey(ri => ri.Id);

            //Configures Area
            modelBuilder.Entity<Area>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Area>()
                .HasOne(a => a.Game)
                .WithMany(g => g.Areas)
                .HasForeignKey(g => g.GameId);

            //Configures RoomConnection
            modelBuilder.Entity<RoomConnection>()
                .HasKey(rc => rc.Id);
            modelBuilder.Entity<RoomConnection>()
                .HasOne(rc => rc.Room1);
            modelBuilder.Entity<RoomConnection>()
                .HasOne(rc => rc.Room2);
        }


        public async Task<User?> GetUserByIdAsnyc(string userId)
        {
            return await Users.FindAsync(userId)
                .ConfigureAwait(false);
        }
        public async Task<MudGame?> GetMudByIdAsnyc(string mudId)
        {
            return await MudGames.FindAsync(mudId)
                .ConfigureAwait(false);
        }
    }
}
