using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIREST_GET.Models
{
    public partial class ARQUITECTURAContext : DbContext
    {
        public ARQUITECTURAContext(DbContextOptions<ARQUITECTURAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MarvelCharacter> MarvelCharacters { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarvelCharacter>(entity =>
            {
                entity.HasKey(e => e.IdeCharacter)
                    .HasName("PK__MARVEL_C__ADAEDAD5CB9CD3EC");

                entity.ToTable("MARVEL_CHARACTERS");

                entity.Property(e => e.IdeCharacter)
                    .HasColumnName("ide_character")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Thumbnail).HasColumnName("thumbnail");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdeUser)
                    .HasName("PK__USERS__98FB428BBDFA2433");

                entity.ToTable("USERS");

                entity.Property(e => e.IdeUser)
                    .HasColumnName("IDE_USER")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Rol)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ROL");

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
