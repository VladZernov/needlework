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
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName).HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.User).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail).HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName).HasName("UserNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

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
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450);
                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Feedback).HasForeignKey(d => d.IdUser).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<UsersPDF>(entity =>
            {
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450);
                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UsersPDF).HasForeignKey(d => d.IdUser).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Palette> Palette { get; set; }
        public virtual DbSet<UsersPDF> UsersPDF { get; set; }
    }
}