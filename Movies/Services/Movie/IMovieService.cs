public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetAllMovies();

    Task<MovieDTO> GetMovie(int id);

    Task<IResult> PostMovie(PostMovieDTO postMovieDTO);

    Task<IEnumerable<MovieDTO>> SearchMovies(string? search);


}