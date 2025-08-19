using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;

public class DirectorService : IDirectorService
{
    private readonly CinemaDbContext _context;

    public DirectorService(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DirectorDTO>> GetAllDirectors()
    {
        return await _context.Directors.Select(d => new DirectorDTO(d)).ToListAsync();
    }

    public async Task<DirectorDTO> GetDirector(int id)
    {
        var director = await _context.Directors.FindAsync(id);

        return new DirectorDTO(director);

    }

    public async Task<IResult> PostDirector(DirectorDTO directorDTO)
    {

        var director = new Director
        {
            Name = directorDTO.Name,
            Surname = directorDTO.Surname,
            Country = directorDTO.Country,
            BirthDate = directorDTO.BirthDate,
        };

        _context.Directors.Add(director);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/directors");

    }
    
    public async Task<IEnumerable<DirectorDTO>> SearchDirectors(string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) { return Enumerable.Empty<DirectorDTO>(); }

        var query = _context.Directors
        .SelectMany(d=> d.Movies, (d, m) => new DirectorDTO
        {
            DirectorId = d.DirectorId,
            Name = d.Name,
            Surname = d.Surname,
            Fullname = d.Name + ' ' + d.Surname,
            Country = d.Country,
            BirthDate = d.BirthDate,
            Movies = d.Movies
                     .Select(m => m.Name)
                     .ToList()
        });

        var tokens = search.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Where(t => t.Length >= 3);

        foreach (var token in tokens)
        {
            var localToken = token; 
            query = query.Where(p =>
                EF.Functions.Like(p.Fullname, $"%{localToken}%") ||
                p.Movies.Any(m => EF.Functions.Like(m, $"%{localToken}%"))||
                EF.Functions.Like(p.Country, $"%{localToken}%") 
            );
        }
        return await query.ToListAsync();        
    }

}