using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace NeedleWork2016.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=DESKTOP-2N2TC81;Database=NeedleWork2016;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Color>(entity =>
            {

                entity.Property(e => e.Hex)
                    .HasMaxLength(7)
                    .HasColumnType("nchar");

                entity.HasOne(d => d.IdPaletteNavigation).WithMany(p => p.ColorList).HasForeignKey(d => d.IdPalette).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Palette>(entity =>
            {
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450);

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Palettes).HasForeignKey(d => d.IdUser).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Palette> Palette { get; set; }
    }
}
