using Microsoft.EntityFrameworkCore;

namespace LaprTrackr.Backend.Models
{
    public class LaprTrackrContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<Eat> EatRecords { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public LaprTrackrContext(DbContextOptions<LaprTrackrContext> optons)
          : base(optons)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDatabaseMaxSize("2 GB");
            modelBuilder.Entity<User>(e => e.HasIndex(x => x.Email).IsUnique(true));
            modelBuilder.Entity<Food>(e => e.HasIndex(x => x.Barcode).IsUnique(true));
            modelBuilder.Entity<RefreshToken>(e => e.HasIndex(x => x.Email));

            base.OnModelCreating(modelBuilder);
        }
    }
}
