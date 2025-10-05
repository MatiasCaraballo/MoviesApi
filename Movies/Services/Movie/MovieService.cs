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

    public async Task<IResult> PostMovie(PostMovieDTO postMovieDTO)
    {

        var movie = new Movie
        {
            Name = postMovieDTO.Name,
            ReleaseYear = postMovieDTO.ReleaseYear,
            Classification = postMovieDTO.Classification,
            Synopsis = postMovieDTO.Synopsis,
            ImdbRating = postMovieDTO.ImdbRating,
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/movies");

    }

    public async Task<IEnumerable<MovieDTO>> SearchMovies(string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) { return Enumerable.Empty<MovieDTO>(); }

        var tokens = search
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(t => t.ToLower())
        .ToArray();

        var query = _context.Movies.AsQueryable();
        
        foreach (var token in tokens)
        {
            var local = token; 
            query = query.Where(m =>
                EF.Functions.Like(m.Name.ToLower(), $"%{local}%") ||
                EF.Functions.Like(m.Synopsis.ToLower(), $"%{local}%") ||
                m.Directors.Any(d =>
                    EF.Functions.Like((d.Name + " " + d.Surname).ToLower(), $"%{local}%")
                )
            );
        }

        var result = await query.Select(m => new MovieDTO
        {
            MovieId = m.MovieId,
            Name = m.Name,
            ReleaseYear = m.ReleaseYear,
            Classification = m.Classification,
            Synopsis = m.Synopsis,
            ImdbRating = m.ImdbRating,
            Directors = m.Directors
                        .Select(d => d.Name + " " + d.Surname)
                        .ToList()
        }).ToListAsync();

        return result;
    }
}