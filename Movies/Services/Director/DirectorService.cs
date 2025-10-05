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

    public async Task<DirectorDTO> GetDirector(int DirectorId)
    {
        var director = await _context.Directors.FindAsync(DirectorId);

        return new DirectorDTO(director);

    }

    public async Task<IResult> PostDirector(DirectorDTO directorDTO)
    {

        var director = new Director
        {
            Name = directorDTO.Name,
            Surname = directorDTO.Surname,
            Country = directorDTO.Country,
            BirthYear = directorDTO.BirthYear,
        };

        _context.Directors.Add(director);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/directors");

    }

    public async Task<IEnumerable<DirectorDTO>> SearchDirectors(string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) { return Enumerable.Empty<DirectorDTO>(); }

        var tokens = search
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Where(t => t.Length >= 3)
        .Select(t => t.ToLower())
        .ToArray();

        var query = _context.Directors.AsQueryable();

        foreach (var token in tokens)
        {
            var local = token;
            query = query.Where(d =>
                EF.Functions.Like(d.Name.ToLower(), $"%{local}%") ||
                EF.Functions.Like(d.Surname.ToLower(), $"%{local}%") ||
                d.Movies.Any(m =>
                    EF.Functions.Like((m.Name).ToLower(), $"%{local}%")
                )
            );
        }

        var result = await query.Select(d => new DirectorDTO
        {
            DirectorId = d.DirectorId,
            Name = d.Name,
            Surname = d.Surname,
            Country = d.Country,
            BirthYear = d.BirthYear
        }).ToListAsync();

        return result;
        
    }

}