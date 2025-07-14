using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
        
    }

    [HttpGet("Movies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Movies")]

    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMovies();
            return Ok(movies);
        }


}