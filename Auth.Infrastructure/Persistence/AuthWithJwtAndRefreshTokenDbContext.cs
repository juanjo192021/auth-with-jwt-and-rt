using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokenHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC0736786F58");

            entity.ToTable("RefreshTokenHistory");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasComputedColumnSql("(case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokenHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__4E88ABD4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC071C5F2412");

            entity.ToTable("User");

            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
