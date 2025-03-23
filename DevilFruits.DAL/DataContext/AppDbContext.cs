using DevilFruits.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevilFruits.DAL.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorito>()
                .HasKey(f => new { f.UsuarioId, f.DevilFruitId });

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Favoritos)
                .HasForeignKey(f => f.UsuarioId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
