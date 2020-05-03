using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDhub.Core.Services
{
    public class MudDbContext : DbContext
    {
        public MudDbContext(DbContextOptions options,
                            IOptions<DatabaseConfiguration> conf = null,
                            ILogger<MudDbContext>? logger = null,
                            bool useInUnitTests = false)
            : base(options)
        {

            if (!useInUnitTests)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            //Configures MudGame
            modelBuilder.Entity<MudGame>()
                .HasKey(mg => mg.Id);

            //Configures MudJoinRequests
            modelBuilder.Entity<MudJoinRequest>()
                .HasKey(mjr => new { mjr.MudId, mjr.UserId });
            modelBuilder.Entity<MudJoinRequest>()
                .HasOne(mjr => mjr.MudGame)
                .WithMany(mg => mg.JoinRequests)
                .HasForeignKey(mjr => mjr.MudId);

            modelBuilder.Entity<User>(
                   b =>
                   {
                       b.HasKey(u => u.Id);
                   });

        }

    }
}
