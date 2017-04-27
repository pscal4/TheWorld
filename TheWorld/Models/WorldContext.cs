using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TheWorld.Models
{
    public class WorldContext : IdentityDbContext<WorldUser>

    {
        private IConfigurationRoot _config;

        public WorldContext( IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
            // EnsureCreated : If the database does not exist, it will create it and apply all migrations
            // However if the database exists, no migrations are applied
            //Database.EnsureCreated();

            // Migrate will create the database if needed.  It also applies any migration that has not been applied yet. 
            Database.Migrate();
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:WorldContextConnection"]);
            
        }
    }
}
