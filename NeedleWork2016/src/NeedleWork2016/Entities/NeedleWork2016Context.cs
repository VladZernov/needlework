using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace NeedleWork2016.Entities
{
    public partial class NeedleWork2016Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=10.10.200.62;Database=NeedleWork2016;Persist Security Info=True;User ID=needlework2016;Password=Qwerty1!");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Color>(entity =>
            {

                entity.Property(e => e.Hex)
                    .HasMaxLength(7)
                    .HasColumnType("nchar");

                entity.HasOne(d => d.IdPaletteNavigation).WithMany(p => p.ColorList).HasForeignKey(d => d.IdPalette).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Palette>(entity =>
            {
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450);

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Palette).HasForeignKey(d => d.IdUser).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Palette> Palette { get; set; }
    }
}