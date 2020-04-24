using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Services
{
    internal class MudDbContext : DbContext, IMudDbContext
    {
        public MudDbContext(DbContextOptions options) 
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; } = null!;
        //ToDo: Moris => Werden Enum in die Datenbank gebracht?

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(
                    b =>
                    {
                        b.HasKey(u => u.Id);
                    });

        }
    }
}
