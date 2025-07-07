using Microsoft.EntityFrameworkCore;

using EFCore.CheckConstraints;
using MoviesApp.Models;

namespace MoviesApp.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }

        public DbSet<MovieDirector> MovieDirectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Name)
                    .IsRequired(true)
                    .HasMaxLength(255);

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

            modelBuilder.Entity<MovieDirector>(entity =>
            {
                entity.ToTable("MovieDirectors");

                // Clave primaria compuesta
                entity.HasKey(md => new { md.DirectorsDirectorId, md.MoviesMovieId });

                // Foreign Key hacia Movie
                entity.HasOne(md => md.Movie)
                      .WithMany(m => m.MovieDirectors)
                      .HasForeignKey(md => md.MoviesMovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign Key hacia Director
                entity.HasOne(md => md.Director)
                      .WithMany(d => d.MovieDirectors)
                      .HasForeignKey(md => md.DirectorsDirectorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
        }
        
        
    }
}