using Microsoft.EntityFrameworkCore;

public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetAllMovies();

    Task<MovieDTO> GetMovie(int id);

    Task<IResult> PostMovie(MovieDTO movieDTO);


}