
using DashboardVentas.API.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

using Microsoft.EntityFrameworkCore;

namespace DashboardVentas.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<venta> Ventas => Set<venta>();
    public DbSet<MetaMensual> MetasMensuales => Set<MetaMensual>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MetaMensual>()
            .HasIndex(m => new { m.Anio, m.Mes })
            .IsUnique();

        modelBuilder.Entity<venta>()
            .Property(v => v.Monto)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<MetaMensual>()
            .Property(m => m.Meta)
            .HasColumnType("decimal(18,2)");
    }
}