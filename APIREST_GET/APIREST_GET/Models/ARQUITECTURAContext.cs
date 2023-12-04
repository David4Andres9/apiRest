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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
