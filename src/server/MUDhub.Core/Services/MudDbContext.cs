using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
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
        }
    }
}
