using Microsoft.EntityFrameworkCore;
using FilmIncelemeProjesi.Models;

namespace FilmIncelemeProjesi.Models
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Filmler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<IletisimMesaji> IletisimMesajlari { get; set; }

        public DbSet<FilmPuani> FilmPuanlari { get; set; }

    }
}
