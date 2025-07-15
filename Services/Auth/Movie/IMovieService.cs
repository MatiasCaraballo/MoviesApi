public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetAllMovies();
        
}