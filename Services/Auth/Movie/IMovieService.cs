public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetAllMovies();

    Task<MovieDTO> GetMovie(int id);
        
}