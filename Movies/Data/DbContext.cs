
using Microsoft.EntityFrameworkCore;

namespace MoviesApp.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Movie entity
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Name)
                    .IsRequired(true)
                    .HasMaxLength(255);

                entity.Property(m => m.ReleaseYear)
                    .IsRequired(true);

                entity.Property(m => m.Classification)
                    .IsRequired(true);

                entity.Property(m => m.Synopsis)
                    .IsRequired(true);
                    
                entity.Property(m => m.ImdbRating)
                    .HasColumnType("decimal(4, 2)")
                    .IsRequired(false);

                entity.HasMany(m => m.Directors)
                    .WithMany(d => d.Movies)
                    .UsingEntity(j => j.ToTable("MovieDirectors"));

                entity.ToTable(t => t.HasCheckConstraint("CK_Movie_ImdbRating", "ImdbRating >= 1.0 AND ImdbRating <= 10.0"));

                entity.ToTable(t => t.HasCheckConstraint("CK_Movie_ReleaseYear", "ReleaseYear >= 1895 AND ReleaseYear <= YEAR(GETDATE()) + 10"));
            });

            //Director entity
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

                entity.Property(d => d.BirthYear)
                    .IsRequired(false);
            });

            //Many to many relationships
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Directors)
                .WithMany(d => d.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieDirectors",
                    j => j
                        .HasOne<Director>()
                        .WithMany()
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Movie>()
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("DirectorId", "MovieId");
                        j.ToTable("MovieDirectors");
                    }
                );

        }


    }
}
        
        

