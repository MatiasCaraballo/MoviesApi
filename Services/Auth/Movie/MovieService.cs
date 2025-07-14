using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
public class movieService : IMovieService
{
    private readonly CinemaDbContext _context;

    public MovieService(CinemaDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<MovieDTO>> GetAllMovies()
    {
        return await _context.Movies.Select(m => new MovieDTO(m)).ToListAsync();
    }

}