using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Services
{
    public class MudDbContext : DbContext
    {
        public MudDbContext(DbContextOptions options, bool useInUnitTests = false)
            : base(options)
        {
            if (!useInUnitTests)
                Database.Migrate();
        }
        public DbSet<User> Users { get; set; } = null!;
        //ToDo: Moris => Werden Enum in die Datenbank gebracht?


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
