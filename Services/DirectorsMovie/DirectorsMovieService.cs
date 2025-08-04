using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
public class DirectorsMovieService : IDirectorsMovieService
{
    private readonly CinemaDbContext _context;
    public DirectorsMovieService(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> GetDirectorsByMovie(int movieId)
    {
        var movie = await _context.Movies
                   .Include(m => m.Directors)
                   .FirstOrDefaultAsync(m => m.MovieId == movieId);

        var result = await _context.Movies.SelectMany(
                        m => m.Directors,
                        (m, d) => new
                        {
                            MovieId = m.MovieId,
                            Movie = m.Name,
                            DirectorId = d.DirectorId,
                            Director = d.Name

                        }
                ).ToListAsync();
        return TypedResults.Ok(result);
    }

    public async Task <IResult> PostMovieDirector(int movieId,int directorId)
    {
        var movie = await _context.Movies
                        .Include(m => m.Directors)
                        .FirstOrDefaultAsync(m => m.MovieId == movieId);

        if (movie == null)
            return Results.NotFound($"Movie {movieId} not found.");

        var director = await _context.Directors.FindAsync(directorId);

        if (director == null)
            return Results.NotFound($"Director {directorId} not found.");

        if (movie.Directors.Any(d => d.DirectorId == directorId))
            return Results.Conflict("The director its asigned to the movie.");

        movie.Directors.Add(director);

        await _context.SaveChangesAsync();

        return Results.Created($"/movie/{movieId}/director/{directorId}", null);

        
    }

}