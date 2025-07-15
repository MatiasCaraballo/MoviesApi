using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
public class MovieService : IMovieService
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

    public async Task<MovieDTO> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        return new MovieDTO(movie);
         
    }

}