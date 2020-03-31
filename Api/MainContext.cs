using CommunicaptionBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Api
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options)
            : base(options){ 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Media> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>().HasKey(x => new { x.userId });
            mb.Entity<Media>().HasKey(x => new { x.mediaId });
        }
    }
}
