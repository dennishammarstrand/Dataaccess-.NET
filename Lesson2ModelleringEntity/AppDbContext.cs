using Lesson2ModelleringEntity.Producer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lesson2ModelleringEntity
{
    public class AppDbContext : DbContext
    {
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Song> Song { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=EFMusic;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasMany(a => a.Albums).WithOne(a => a.Artist);

            modelBuilder.Entity<Album>().HasMany(s => s.Songs).WithOne(s => s.Album);

            modelBuilder.Entity<Song>().HasOne(a => a.Album);

            modelBuilder.Entity<AlbumProducers>().HasKey(p => new { p.AlbumID, p.ProducerID });
        }

        public static async Task LoadDbOnStart()
        {
            await Program.database.Artist.FindAsync(1);
        }
    }
}
