using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WSVenta.Models;

public partial class VentaRealContext : DbContext
{
    public VentaRealContext()
    {
    }

    public VentaRealContext(DbContextOptions<VentaRealContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=DESKTOP-PFQ1Q9C;Database=VentaReal;Trusted_Connection=True; Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.ToTable("concepto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Importe)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("importe");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("precioUnitario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Conceptos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_concepto_producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Conceptos)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_concepto_venta");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("producto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(16, 0)")
                .HasColumnName("costo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("precioUnitario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.ToTable("venta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_venta_cliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
