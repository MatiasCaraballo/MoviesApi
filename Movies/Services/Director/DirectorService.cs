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

}