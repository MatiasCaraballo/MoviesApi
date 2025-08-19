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

        return TypedResults.Created($"/movies");

    }

    public async Task<IEnumerable<MovieDTO>> SearchMovies(string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) { return Enumerable.Empty<MovieDTO>(); }

        var query = _context.Movies
        .SelectMany(m => m.Directors, (m, d) => new MovieDTO
        {
            MovieId = m.MovieId,
            Name = m.Name,
            ReleaseDate = m.ReleaseDate,
            Classification = m.Classification,
            Synopsis = m.Synopsis,
            ImdbRating = m.ImdbRating,
            Directors = m.Directors
                     .Select(d => d.Name + " " + d.Surname)
                     .ToList()
        });

        var tokens = search.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Where(t => t.Length >= 3);

        foreach (var token in tokens)
        {
            var localToken = token; 
            query = query.Where(p =>
                EF.Functions.Like(p.Name, $"%{localToken}%") ||
                p.Directors.Any(d => EF.Functions.Like(d, $"%{localToken}%"))
            );
        }

        return await query.ToListAsync();        
    }
}