using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
public class MovieService : IMovieService
{
    private readonly CinemaDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MovieService(CinemaDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<IResult> PostMovie(MovieDTO movieDTO)
    {
        
        var movie = new Movie
        {
            Name = movieDTO.Name,
            ReleaseDate = movieDTO.ReleaseDate,
            Classification = movieDTO.Classification,
            ImdbRating = movieDTO.ImdbRating,
            CreatedAt = DateTime.Now
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/movies/{movieDTO}");
            
    }

}