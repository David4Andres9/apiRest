using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIREST_GET.Models
{
    public partial class ARQUITECTURAContext : DbContext
    {
        public ARQUITECTURAContext()
        {
        }

        public ARQUITECTURAContext(DbContextOptions<ARQUITECTURAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MarvelCharacter> MarvelCharacters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarvelCharacter>(entity =>
            {
                entity.ToTable("MARVEL_CHARACTERS");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ConsultNumber).HasColumnName("CONSULT_NUMBER");

                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name).HasColumnName("NAME");

                entity.Property(e => e.Thumbnail).HasColumnName("THUMBNAIL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
