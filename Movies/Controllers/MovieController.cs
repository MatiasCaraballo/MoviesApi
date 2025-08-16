using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/movies")]
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
    [Authorize]


    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
    {
        try
        {
            var movies = await _movieService.GetAllMovies();
            return Ok(movies);

        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");

        }
    }

    [HttpGet("Movies/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Movies")]
    [Authorize]
    
    public async Task<ActionResult<MovieDTO>> GetMovie(int id)
    {
        try
        {
            var movie = await _movieService.GetMovie(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }

    [HttpPost("Movies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Movies")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDTO)
    {
        try
        {
            var movie = await _movieService.PostMovie(movieDTO);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }



}