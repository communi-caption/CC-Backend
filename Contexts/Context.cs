using CommunicaptionBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Contexts
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options){ 
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>().HasKey(x => new { x.userId });
        }
    }
}
