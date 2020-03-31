using CommunicaptionBackend.Entities;
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

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MediaEntity> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<UserEntity>().HasKey(x => new { x.UserId });
            mb.Entity<MediaEntity>().HasKey(x => new { x.MediaId });
        }
    }
}
