using Microsoft.EntityFrameworkCore;
using MeuProjeto.Models;

namespace MeuProjeto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Perfil> Perfis { get; set; }

        public DbSet<SolicitacaoPerfil> SolicitacoesPerfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Perfil>().HasData(
                new Perfil
                {
                    Id = 1,
                    Nome = "Admin"
                },
                new Perfil
                {
                    Id = 2,
                    Nome = "Usuario"
                }
            );
        }
    }
}