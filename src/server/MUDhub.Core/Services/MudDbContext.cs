using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
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
                try
                {
                    Database.Migrate();
                }
                catch (SqliteException e)
                {
                    logger?.LogWarning(e, "The Server has a new Data schema Version, some providers(e.g. sqlite) don't support some Migrations operations the old database will be delete and a new will be created.");
                    Database.EnsureDeleted();
                    Database.EnsureCreated();
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
