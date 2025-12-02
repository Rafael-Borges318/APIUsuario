using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var usuario = modelBuilder.Entity<Usuario>();

        usuario.HasKey(u => u.Id);

        usuario.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        usuario.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        usuario.HasIndex(u => u.Email)
            .IsUnique();

        usuario.Property(u => u.Senha)
            .IsRequired()
            .HasMaxLength(200);

        usuario.Property(u => u.DataNascimento)
            .IsRequired();

        usuario.Property(u => u.Telefone)
            .HasMaxLength(20);

        usuario.Property(u => u.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        usuario.Property(u => u.DataCriacao)
            .IsRequired();
    }
}
