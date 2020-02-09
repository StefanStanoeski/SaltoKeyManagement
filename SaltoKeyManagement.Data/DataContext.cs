using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaltoKeyManagement.Models.Domain;

namespace SaltoKeyManagement.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Door> DoorsDbSet { get; set; }
        public DbSet<Entrance> EntrancesDbSet { get; set; }
        public DbSet<RefreshToken> RefreshTokensDbSet { get; set; }
    }
}
