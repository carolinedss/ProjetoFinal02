using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Projeto02.Models;

namespace Projeto02.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Colaborador> Colaboradors { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<Epi> Epis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=epi;UserId=postgres;Password=senai091;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.HasKey(e => e.CodigoColab).HasName("colaborador_pkey");

            entity.ToTable("colaborador");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.HasIndex(e => e.CodigoColab, "idx_codigo_colab");

            entity.HasIndex(e => e.Telefone, "idx_telefone");

            entity.HasIndex(e => e.Telefone, "telefone").IsUnique();

            entity.Property(e => e.CodigoColab)
                .ValueGeneratedNever()
                .HasColumnName("codigo_colab");
            entity.Property(e => e.Cpf).HasColumnName("cpf");
            entity.Property(e => e.Ctps)
                .HasMaxLength(20)
                .HasColumnName("ctps");
            entity.Property(e => e.Dtadmissao).HasColumnName("dtadmissao");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .HasColumnName("email");
            entity.Property(e => e.Gerente)
                .HasMaxLength(120)
                .HasColumnName("gerente");
            entity.Property(e => e.Nome)
                .HasMaxLength(120)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.CodigoEntrega).HasName("entrega_pkey");

            entity.ToTable("entrega");

            entity.HasIndex(e => e.CodigoEntrega, "idx_codigo_entrega");

            entity.Property(e => e.CodigoEntrega)
                .ValueGeneratedNever()
                .HasColumnName("codigo_entrega");
            entity.Property(e => e.CodigoColab).HasColumnName("codigo_colab");
            entity.Property(e => e.CodigoEpi).HasColumnName("codigo_epi");
            entity.Property(e => e.DtEntrega).HasColumnName("dt_entrega");
            entity.Property(e => e.DtValidade).HasColumnName("dt_validade");

            entity.HasOne(d => d.CodigoEntregaNavigation).WithOne(p => p.Entrega)
                .HasForeignKey<Entrega>(d => d.CodigoEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("codigo_colab");

            entity.HasOne(d => d.CodigoEpiNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.CodigoEpi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("codigo_epi");
        });

        modelBuilder.Entity<Epi>(entity =>
        {
            entity.HasKey(e => e.CodigoEpi).HasName("epi_pkey");

            entity.ToTable("epi");

            entity.HasIndex(e => e.CodigoEpi, "idx_codigo_epi");

            entity.Property(e => e.CodigoEpi)
                .ValueGeneratedNever()
                .HasColumnName("codigo_epi");
            entity.Property(e => e.FormaDu).HasColumnName("forma_du");
            entity.Property(e => e.Nome)
                .HasMaxLength(120)
                .HasColumnName("nome");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
