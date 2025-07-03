using Microsoft.EntityFrameworkCore;

namespace MoviesApp.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Genre);
                entity.Property(m => m.ReleaseDate);

                entity
                 .HasMany(m => m.Directors)
                 .WithMany(d => d.Movies)
                 .UsingEntity(j => j.ToTable("MovieDirectors"));
             });
            /*base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Synopsis).IsRequired();
                entity.Property(m => m.RunningTime).IsRequired();
                entity.Property(m => m.ReleaseDate).IsRequired();
                entity.Property(m => m.Genre).IsRequired().HasMaxLength(50);
            });*/
        }
    }
}