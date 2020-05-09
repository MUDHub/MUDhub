using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Models.Users;
using System;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class MudDbContext : DbContext
    {
        public MudDbContext(DbContextOptions options,
                            IOptions<DatabaseConfiguration>? conf = null,
                            ILogger<MudDbContext>? logger = null,
                            bool useNotInUnitests = true)
            : base(options)
        {

            if (useNotInUnitests)
            {
               

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
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<ItemInstance> ItemInstances { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 

            //Configures MudGame
            modelBuilder.Entity<MudGame>()
                .HasKey(mg => mg.Id);
            modelBuilder.Entity<MudGame>()
                .HasMany(mg => mg.Characters)
                .WithOne(c => c.Game);
            modelBuilder.Entity<MudGame>()
                .HasMany(g => g.Areas)
                .WithOne(a => a.Game)
                .HasForeignKey(a => a.GameId);
            modelBuilder.Entity<MudGame>()
                .HasMany(g => g.Classes)
                .WithOne(c => c.Game)
                .HasForeignKey(c => c.GameId);
            modelBuilder.Entity<MudGame>()
                .HasMany(g => g.Races)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId);

            //Configures Character
            modelBuilder.Entity<Character>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Race)
                .WithMany(r => r.Characters);
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Class)
                .WithMany(cl => cl.Characters);

            modelBuilder.Entity<Character>()
                .HasOne(c => c.ActualRoom)
                .WithMany(r => r.Characters);

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
                .HasForeignKey(mjr => mjr.MudId)
                .OnDelete(DeleteBehavior.Cascade);

            //Configures User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            //Configures Room
            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Area)
                .WithMany(a => a.Rooms)
                .HasForeignKey(r => r.AreaId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Game)
                .WithMany()
                .HasForeignKey(r => r.GameId);
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Interactions)
                .WithOne(i => i.Room);

            //Configures RoomInteraction
            modelBuilder.Entity<RoomInteraction>()
                .HasKey(ri => ri.Id);

            //Configures Area
            modelBuilder.Entity<Area>()
                .HasKey(a => a.Id);


            //Configures RoomConnection
            modelBuilder.Entity<RoomConnection>()
                .HasKey(rc => rc.Id);

            modelBuilder.Entity<RoomConnection>()
                .HasOne(rc => rc.Room1)
                .WithMany(r => r.Connections)
                .HasForeignKey(rc => rc.Room1Id);

            modelBuilder.Entity<RoomConnection>()
                .HasOne(rc => rc.Room2)
                .WithMany()
                .HasForeignKey(rc => rc.Room2Id);

            //Configures Item
            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            //Configures ItemInstance
            modelBuilder.Entity<ItemInstance>()
                .HasKey(ii => ii.Id);

            //Configure Inventory
            modelBuilder.Entity<Inventory>()
                .HasKey(it => it.Id);
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
