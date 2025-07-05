using Microsoft.EntityFrameworkCore;

using EFCore.CheckConstraints;

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

                entity.Property(m => m.Name)
                    .IsRequired(true)
                    .HasMaxLength(255);

                /*Genre most be in a many to many relationship
                entity.Property(m => m.Genre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty);
                */
                entity.Property(m => m.ReleaseDate)
                    .IsRequired(false);

                entity.Property(m => m.Classification)
                    .IsRequired(true);

                entity.Property(m => m.CreatedAt)
                    .IsRequired(false);

                entity.Property(m => m.ImdbRating)
                    .HasColumnType("decimal(4, 2)")
                    .IsRequired(false);

                entity.HasMany(m => m.Directors)
                    .WithMany(d => d.Movies)
                    .UsingEntity(j => j.ToTable("MovieDirectors"));

                entity.ToTable(t => t.HasCheckConstraint("CK_Movie_ImdbRating", "ImdbRating >= 1.0 AND ImdbRating <= 10.0"));
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.HasKey(d => d.DirectorId);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(d => d.Surname)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(d => d.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.BirthDate)
                    .IsRequired(false);

                entity.Property(d => d.CreatedAt)
                    .IsRequired(false);
            });
        }
    }
}