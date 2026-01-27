using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GlobusT.Data;

public partial class GlobusTechnologyContext : DbContext
{
    public GlobusTechnologyContext()
    {
    }

    public GlobusTechnologyContext(DbContextOptions<GlobusTechnologyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<DevelopmentArea> DevelopmentAreas { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<TypeCommand> TypeCommands { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GlobusTechnology;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DevelopmentArea>(entity =>
        {
            entity.ToTable("DevelopmentArea");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Request");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_Service");

            entity.HasOne(d => d.IdStatusRequestNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdStatusRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_RequestStatus");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_User");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.ToTable("RequestStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Service");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.IdDevelopmentAreaNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdDevelopmentArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Service_DevelopmentArea");

            entity.HasOne(d => d.IdTypeCommandNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTypeCommand)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Service_TypeCommand");
        });

        modelBuilder.Entity<TypeCommand>(entity =>
        {
            entity.ToTable("TypeCommand");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.ContactEmal).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Company");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
