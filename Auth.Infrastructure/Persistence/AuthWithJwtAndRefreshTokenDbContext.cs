using System;
using System.Collections.Generic;
using Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Entities;

namespace Auth.Infrastructure.Persistence;

public partial class AuthWithJwtAndRefreshTokenDbContext : DbContext
{
    public AuthWithJwtAndRefreshTokenDbContext()
    {
    }

    public AuthWithJwtAndRefreshTokenDbContext(DbContextOptions<AuthWithJwtAndRefreshTokenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshTokenHistory> RefreshTokenHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokenHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07C7BEF687");

            entity.ToTable("RefreshTokenHistory");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasComputedColumnSql("(case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokenHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__70DDC3D8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070DBC8169");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__737584F6C1840E36").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC075EC665AE");

            entity.ToTable("User");

            entity.HasIndex(e => e.DocumentNumber, "UQ__User__689939186398324D").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534538EA207").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true).ValueGeneratedOnAdd();
            entity.Property(e => e.IsConfirmed).HasDefaultValue(true).ValueGeneratedOnAdd();
            entity.Property(e => e.LastName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserType");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__UserRole__AF2760AD01197A8D");

            entity.ToTable("UserRole");

            entity.Property(e => e.IsActive).HasDefaultValue(true).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__RoleId__47DBAE45");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__UserId__46E78A0C");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserType__3214EC07F5B50D41");

            entity.ToTable("UserType");

            entity.HasIndex(e => e.Name, "UQ__UserType__737584F62AB3BE4A").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
