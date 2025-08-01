using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace MoviesApp.Data
{
    public class CinemaDbContext : IdentityDbContext<AppUser>
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

                entity.Property(m => m.ReleaseDate)
                    .IsRequired(false);

                entity.Property(m => m.Classification)
                    .IsRequired(true);

                entity.Property(m => m.Synopsis)
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

                entity.Property(d => d.BirthDate)
                    .IsRequired(false);

                entity.Property(d => d.CreatedAt)
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
            modelBuilder.Entity<AppUser>()
                   .HasIndex(u => u.Email)
                   .IsUnique();

            modelBuilder.Entity<AppUser>()
                   .HasIndex(u => u.NormalizedUserName)
                   .IsUnique();
        }

            
    }
}
        
        

