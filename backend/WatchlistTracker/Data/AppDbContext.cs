using Microsoft.EntityFrameworkCore;
using WatchlistTracker.Models;

namespace WatchlistTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<PeliculaSerie> PeliculasSeries { get; set; }
        public DbSet<ListaContenido> ListasContenidos { get; set; }

    }
}
