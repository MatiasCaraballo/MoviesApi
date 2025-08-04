public interface IDirectorsMovieService
{
    Task<IResult> GetDirectorsByMovie(int id);

    Task<IResult> PostMovieDirector(int movieId, int directorId);

}