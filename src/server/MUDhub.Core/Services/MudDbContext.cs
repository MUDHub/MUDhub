using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDhub.Core.Services
{
    public class MudDbContext : DbContext
    {
        public MudDbContext(DbContextOptions options,
                            ILogger<MudDbContext>? logger = null,
                            bool useInUnitTests = false)
            : base(options)
        {

            if (!useInUnitTests)
            {
                if (Database.IsSqlite())
                {
                    var result = Database.GetPendingMigrations().ToList();
                    if (Database.GetPendingMigrations().FirstOrDefault() != null)
                    {
                        logger?.LogWarning("The Server has a new Data schema Version, sqlite don't support some Migrations operations. " +
                                            "The old database will be delete and a new will be created.");
                        Database.EnsureDeleted();
                        Database.EnsureCreated();
                    }
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
