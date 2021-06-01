using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlQueryBuilder.Infrastructure
{
    public class Cinema
    {
        public int CinemaId { get; set; }

        public int LocationId { get; set; }
        
        public string Name { get; set; }
        
        public string Owner { get; set; }
        
        public string Rating { get; set; }
        
        public string Telephone { get; set; }
        
        public string Website { get; set; }
    }

    public sealed class BaseDbContext : DbContext
    {
        public DbSet<Cinema> Cinema { get; set; }

        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
